using AmazonTool.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace AmazonTool.Util
{
    public class TokenBiz
    {
        public static string GetAuthUrl(string userId)
        {
            string ApplicationId = string.Empty, ClientSecret = string.Empty;
            ApplicationId = ApiSetting.AmazonSp.ApplicationId;
            string strKey = HttpUtility.UrlEncode(WebSite.WebRootPath + "/ApiUser/ReceiveToken" + "|" + userId).Replace("+", "%20");
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("application_id", ApplicationId);
            //param.Add("scope", "cpc_advertising:campaign_management");
            //param.Add("response_type", "code");
            param.Add("state", strKey);
            param.Add("redirect_uri", "https://www.sumool.com/ReceiveAmazonToken.html");
            param.Add("version", "beta");//表示是测试APP
            //string signature = LazadaUtil.SignStr("", param, appSignatrue);
            string urlParam = string.Empty;
            foreach (string key in param.Keys)
            {
                if (string.IsNullOrEmpty(urlParam))
                    urlParam = key + "=" + param[key];
                else
                    urlParam += "&" + key + "=" + param[key];
            }
            //urlParam += "&_aop_signature=" + signature;
            string url = "https://sellercentral.amazon.com/apps/authorize/consent?" + urlParam;
            return url;
        }
        /// <summary>
        /// 通过code换取token
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public static string GetAccesskey(Dictionary<string, object> queryParam)
        {
            var result = string.Empty;
            try
            {
                string code = queryParam.GetStringValue("Code");
                string scope = queryParam.GetStringValue("selling_partner_id");
                string ClientId = string.Empty, ClientSecret = string.Empty;
                ClientId = ApiSetting.AmazonSp.ClientId;
                ClientSecret = ApiSetting.AmazonSp.ClientSecret;
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("client_id", ClientId);
                param.Add("grant_type", "authorization_code");
                param.Add("redirect_uri", "https://www.sumool.com/ReceiveAmazonToken.html");
                param.Add("client_secret", ClientSecret);
                param.Add("code", code);
                var Authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ClientId, ClientSecret)));
                result = NewAmazonUtil.TokenRequestAPI("https://api.amazon.com/auth/o2/token", param, Authorization: Authorization);
                if (result.HasError)
                {
                    return result;
                }
                JObject jobject = JObject.Parse(result.ResultObject.ToString());
                var access_token = jobject.GetStringValue("access_token");
                if (!string.IsNullOrEmpty(access_token))
                {
                    string refreshToken = jobject.GetStringValue("refresh_token");
                    int expiresIn = jobject.GetIntValue("expires_in");//令牌过期时间
                    string token_type = jobject.GetStringValue("token_type");//token_type
                    //开始保存授权
                    JObject returnObject = new JObject();
                    returnObject.Add("access_token", access_token);
                    returnObject.Add("expires_in", DateTime.Now.AddSeconds(expiresIn).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                    returnObject.Add("refresh_token", refreshToken);
                    returnObject.Add("refresh_token_timeout", DateTime.Now.AddYears(1).ToString("yyyy-MM-dd HH:mm:ss"));
                    result.ResultObject = JsonConvert.SerializeObject(returnObject);

                }
                else
                {
                    result.HasError = true;
                    result.Message = "授权失败：" + jobject.GetStringValue("error_description");
                }
            }
            catch (Exception e)
            {
                result = e.ToOperateResult();
            }
            return result;
        }




        /// <summary>
        /// 刷新授权令牌
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static OperateResult ReloadAccessToken(ref AmazonAccount account)
        {
            OperateResult result = new OperateResult();
            try
            {
                string ClientId = string.Empty, ClientSecret = string.Empty;
                ClientId = ApiSetting.AmazonSp.ClientId;
                ClientSecret = ApiSetting.AmazonSp.ClientSecret;
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("client_id", ClientId);
                param.Add("client_secret", ClientSecret);
                param.Add("grant_type", "refresh_token");
                param.Add("refresh_token", account.RefreshToken);
                var Authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ClientId, ClientSecret)));
                result = NewAmazonUtil.TokenRequestAPI("https://api.amazon.com/auth/o2/token", param, Authorization: Authorization);
                if (result.HasError)
                {
                    return result;
                }
                JObject token = JObject.Parse(result.ResultObject.ToString());
                var access_token = token.GetStringValue("access_token");
                if (!string.IsNullOrEmpty(access_token))
                {
                    string sellerId = account.SellerId;
                    account.AccessToken = access_token;
                    var refresh_token = token.GetStringValue("refresh_token");
                    account.RefreshToken = refresh_token;
                    int expiresIn = token.GetIntValue("expires_in");//令牌过期时间
                    account.TokenExpiresDate = DateTime.Now.AddSeconds(expiresIn).AddMinutes(-1);
                    using (var db = new EntityErpContainer())
                    {
                        var dbApiUserList = db.ErpApiUser.Where(m => m.UserFrom == SaleApiType.Amazon && m.ApiUserColumn2 == sellerId).ToList();
                        foreach (var item in dbApiUserList)
                        {
                            item.ApiUserColumn8 = access_token;
                            item.ApiUserColumn9 = DateTime.Now.AddSeconds(expiresIn).AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
                            item.ApiUserColumn11 = refresh_token;
                            if (string.IsNullOrEmpty(item.ApiUserColumn12))
                            {
                                item.ApiUserColumn12 = DateTime.Now.AddDays(360).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    result.HasError = true;
                    result.Message = token.GetStringValue("error_description");
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取授权
        /// </summary>
        public static OperateResult GetMWSAuthToken(AmazonAccount account)
        {
            OperateResult result = new OperateResult();
            string command = "/sellers/v1/marketplaceParticipations";
            Dictionary<string, object> param = new Dictionary<string, object>();
            result = NewAmazonUtil.RequestAPI(account, command, param, "get");
            if (result.HasError)
            {
                throw new Exception("授权失败，请检查输入的账号信息是否正确");
            }
            return result;
        }



        /// <summary>
        /// 获取PII数据请求令牌
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static OperateResult GetrestrictedDataToken(ref AmazonAccount account, string orderNo = "")
        {
            OperateResult result = new OperateResult();
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("targetApplication", ApiSetting.AmazonSp.ApplicationId);
                JArray restrictedResources = new JArray();
                JObject oneRestrictedResource = new JObject();
                oneRestrictedResource.Add("method", "GET");
                if (string.IsNullOrEmpty(orderNo))
                {
                    oneRestrictedResource.Add("path", "/orders/v0/orders");
                }
                else
                {
                    oneRestrictedResource.Add("path", "/orders/v0/orders/" + orderNo);
                }
                JArray dataElements = new JArray();
                dataElements.Add("shippingAddress");
                dataElements.Add("buyerInfo");
                oneRestrictedResource.Add("dataElements", dataElements);
                restrictedResources.Add(oneRestrictedResource);
                data.Add("restrictedResources", restrictedResources);
                string command2 = "/tokens/2021-03-01/restrictedDataToken";
                result = NewAmazonUtil.RequestAPI(account, command2, data);
                if (result.HasError)
                {
                    return result;
                }
                //{"expiresIn":3600,"restrictedDataToken":"JLJKLKJL"}
                var resultJob = JObject.Parse(result.ResultObject.ToString());
                if (resultJob != null)
                {
                    var piiToken = resultJob.GetStringValue("restrictedDataToken");//过期时间设置为  
                    if (!string.IsNullOrEmpty(piiToken))
                    {
                        if (!string.IsNullOrEmpty(orderNo))
                        {
                            //指定订单的不需要存到数据库
                            result.PutValue("PiiToken", piiToken);
                            return result;
                        }
                        else
                        {
                            using (var db = new EntityErpContainer())
                            {
                                string sellerId = account.SellerId;
                                var dbApiUserList = db.ErpApiUser.Where(m => m.UserFrom == SaleApiType.Amazon && m.ApiUserColumn2 == sellerId).ToList();
                                foreach (var item in dbApiUserList)
                                {
                                    item.ApiUserColumn16 = piiToken;
                                    item.ApiUserColumn17 = DateTime.Now.AddSeconds(3520).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                account.TrestrictedDataTokenExpiresDate = DateTime.Now.AddSeconds(3520);
                                account.TrestrictedDataToken = piiToken;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    result.HasError = true;
                    result.Message = "刷新PII数据访问令牌异常";
                }
            }
            catch (Exception e)
            {
                result = e.Message();
            }
            return result;
        }


        /// <summary>
        /// 获取数据库最新店铺信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static ErpApiUser GetNewestApiUser(AmazonAccount account)
        {
            try
            {
                using (var db = new EntityErpContainer())
                {
                    return db.ErpApiUser.AsNoTracking().Where(m => m.UserFrom == SaleApiType.Amazon && m.UserId == account.UserId).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                e.ToOperateResult();
                return null;
            }
        }
    }
}
