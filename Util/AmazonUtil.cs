using AmazonTool.Model;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AmazonTool.Util
{
    public static class AmazonUtil
    {
        private static string awsSelecetId = "";
        private static string awsSelecetKey = "";

        /// <summary>
        /// 获取端口地址
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static string? GetEndpoint(AmazonAccount account)
        {
            var markeyplaceArray = account.MarketplaceId.Split(',');
            var amazonSite_RegionCode = "";// 可以做一个拓展方法或者数据维护实现

            //var amazonSite = GetAmazonSiteInfoList().Where(m => markeyplaceArray.Contains(m.MarketplaceId)).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(amazonSite_RegionCode))
            {
                return null;
            }
            switch (amazonSite_RegionCode)
            {
                case "na":
                    return "https://sellingpartnerapi-na.amazon.com";//北美
                case "eu":
                    return "https://sellingpartnerapi-eu.amazon.com";//欧洲
                case "fe":
                    return "https://sellingpartnerapi-fe.amazon.com";//东亚
            }
            return null;
        }

        static byte[] HmacSHA256(String data, byte[] key)
        {
            String algorithm = "HmacSHA256";
            KeyedHashAlgorithm kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;

            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        static byte[] getSignatureKey(String key, String dateStamp, String regionName, String serviceName)
        {
            byte[] kSecret = Encoding.UTF8.GetBytes(("AWS4" + key).ToCharArray());
            byte[] kDate = HmacSHA256(dateStamp, kSecret);
            byte[] kRegion = HmacSHA256(regionName, kDate);
            byte[] kService = HmacSHA256(serviceName, kRegion);
            byte[] kSigning = HmacSHA256("aws4_request", kService);

            return kSigning;
        }
        private static string ToHex(byte[] bytes, bool upperCase = false)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            return result.ToString();
        }

        private static string SHA256HexHashString(string StringIn)
        {
            string hashString;
            using (var sha256 = SHA256Managed.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(StringIn));
                hashString = ToHex(hash, false);
            }
            return hashString;
        }

        /// <summary>
        /// 根据提供的参数，请求API并返回数据
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="command">API接口名称</param>
        /// <param name="param">请求参数</param>
        /// <param name="returnType">返回类型</param>
        /// <param name="isNeedPiiToken">是否需要专门使用PIItoken访问</param>
        /// <param name="piiToken">传入单独的PIItoken</param>
        /// <returns></returns>
        public static string RequestAPI(AmazonAccount account, string command, Dictionary<string, object> param, string type = "post", string paramType = "", bool isNeedPiiToken = false, string piiToken = "")
        {
            var result = string.Empty;
            bool isNeedRefToken = false;//需要刷新一下token
            bool isNeedRefPIIToken = false;//是否需要刷新PIItoken
            bool isOtherdeniedRef = false;//其他异常提示刷新授权码
            int refPIITokenCount = 0;//刷了多少次
            int refCount = 0;//刷了多少次
            int reftokenTime = 0;
            try
            {
                string ppdata = "";
                if (param.ContainsKey("smpostbody"))
                {
                    ppdata = JsonConvert.SerializeObject(param["smpostbody"]);
                    param.Remove("smpostbody");
                }
            doRefToken:
                if (string.IsNullOrEmpty(account.AccessToken) || account.TokenExpiresDate < DateTime.Now || isNeedRefToken)
                {
                    refCount++;
                    result = TokenBiz.ReloadAccessToken(ref account);
                    if (result.HasError)
                    {
                        return result;
                    }
                }
            doRefppiitoken:
                string urlPart = GetEndpoint(account);
                String dateStamp = DateTime.Now.ToUniversalTime().ToString("yyyyMMdd");
                String regionName = GetRegionName(urlPart);
                String serviceName = "execute-api";
                string xamzdate = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                string postData = "";
                Dictionary<string, object> paramss = new Dictionary<string, object>();
                if (param.Keys.Count() > 0 && type != "get")
                {
                    if (paramType == "Query")
                    {
                        paramss = param;
                        postData = ppdata;
                    }
                    else
                    {

                        postData = JsonConvert.SerializeObject(param);
                    }
                }
                string postDataHash = SHA256HexHashString(postData);
                if (type == "get")
                {
                    paramss = param;
                }
                //paramss.Add("Action", "ListUsers");
                //paramss.Add("Version", "2010-05-08");
                Dictionary<string, string> hearderDic = new Dictionary<string, string>();
                Uri endpoint = new Uri(urlPart.ToLower());
                hearderDic.Add("host", endpoint.Host);
                if (!string.IsNullOrEmpty(piiToken))
                {
                    hearderDic.Add("x-amz-access-token", piiToken);
                }
                else if (isNeedPiiToken)
                {
                    //判断一下PIItoken是否过期
                    if (string.IsNullOrEmpty(account.TrestrictedDataToken) || account.TrestrictedDataTokenExpiresDate < DateTime.Now || isNeedRefPIIToken)
                    {
                        refCount++;
                        result = TokenBiz.GetrestrictedDataToken(ref account);
                        if (result.HasError)
                        {
                            if ((result.Message ?? "").Contains("Size of a request header field exceeds server limit"))
                            {
                                hearderDic.Add("x-amz-access-token", account.AccessToken);
                                goto alsoPost;
                            }
                            result.Message = "刷新PiiToken出错：" + result.Message;
                            return result;
                        }
                    }
                    hearderDic.Add("x-amz-access-token", account.TrestrictedDataToken);
                }
                else
                {
                    hearderDic.Add("x-amz-access-token", account.AccessToken);
                }
            alsoPost:
                hearderDic.Add("x-amz-date", xamzdate);
                string postDataHash3 = SHA256HexHashString(CalculateStringToSignV4(paramss, hearderDic, string.Join(";", hearderDic.OrderBy(M => M.Key).Select(M => M.Key).ToList()), postDataHash, "", command, type.ToUpper()));
                string StringToSign = "";
                StringToSign += "AWS4-HMAC-SHA256\n";
                StringToSign += xamzdate.Replace(":", "").Replace("-", "") + "\n";
                StringToSign += dateStamp + "/" + regionName + "/" + serviceName + "/aws4_request\n" + postDataHash3;
                var kSigning = getSignatureKey(awsSelecetKey, dateStamp, regionName, serviceName);
                var signature = ToHex(HmacSHA256(StringToSign, kSigning));
                string Authorization = "AWS4-HMAC-SHA256 Credential=" + awsSelecetId + "/" + dateStamp + "/" + regionName + "/" + serviceName + "/aws4_request";
                Authorization += ",SignedHeaders=host;x-amz-access-token;x-amz-date";
                Authorization += ",Signature=" + signature;
                hearderDic.Add("Authorization", Authorization);
                //Authorization: AWS4-HMAC-SHA256 Credential=AKIAIHV6HIXXXXXXX/20201022/us-east-1/execute-api/aws4_request, SignedHeaders=host;user-agent;x-amz-access-token,
                //Signature=5d672d79c15b13162d9279b0855cfba6789a8edb4c82c400e06b5924aEXAMPLE
                //开始请求接口
                string url = urlPart + command;
                hearderDic.Remove("host");
                if (type == "get")
                {
                    string urlParam = string.Empty;
                    foreach (string key in param.Keys)
                    {
                        if (string.IsNullOrEmpty(urlParam))
                            urlParam = UrlEncode(key, false) + "=" + UrlEncode(param[key].ToString(), false);
                        else
                            urlParam += "&" + UrlEncode(key, false) + "=" + UrlEncode(param[key].ToString(), false);
                    }
                    result = NewHttpUtil.Get(url, urlParam, hearderDic: hearderDic);
                }
                else if (paramType == "Query")
                {
                    string urlParam = string.Empty;
                    foreach (string key in param.Keys)
                    {
                        if (string.IsNullOrEmpty(urlParam))
                            urlParam = UrlEncode(key, false) + "=" + UrlEncode(param[key].ToString(), false);
                        else
                            urlParam += "&" + UrlEncode(key, false) + "=" + UrlEncode(param[key].ToString(), false);
                    }
                    result = NewHttpUtil.Post(url + "?" + urlParam, ppdata, hearderDic: hearderDic, Method: type.ToUpper());
                }
                else
                {
                    result = NewHttpUtil.Post(url, postData, hearderDic: hearderDic, Method: type.ToUpper());
                }
                if (result.HasError)
                {
                    return result;
                }
                if (result.ResultObject == null)
                {
                    result.HasError = true;
                    result.Message = "连接超时！请稍后再试";
                }
                if (result.ResultObject.ToString().Contains("errors"))
                {
                    var resultObject = JObject.Parse(result.ResultObject.ToString());
                    var errors = resultObject.GetJArrayValue("errors");
                    var errorOne = resultObject.GetJObjectValue("errors");
                    if (errors == null && errorOne != null)
                    {
                        errors = new JArray();
                        errors.Add(errorOne);
                    }
                    if (errors != null)
                    {
                        result.HasError = true;
                        List<string> errorList = new List<string>();
                        foreach (JObject errorItem in errors)
                        {
                            var msg = errorItem.GetStringValue("message");
                            var code = errorItem.GetStringValue("code");
                            var details = errorItem.GetStringValue("details");
                            if (details.Contains("expired") && refCount < 4)//过期就需要刷新，防止一直刷新添加个计数
                            {
                                if (isNeedPiiToken)
                                {
                                    if (string.IsNullOrEmpty(piiToken) && refPIITokenCount < 4)
                                    {
                                        refPIITokenCount++;
                                        isNeedRefPIIToken = true;
                                        goto doRefppiitoken;
                                    }
                                }
                                else
                                {
                                    //表示超时了，超时的话需要刷新授权码
                                    isNeedRefToken = true;
                                    goto doRefToken;
                                }
                            }
                            if (details.Contains("Access to requested resource is denied") && !isOtherdeniedRef)
                            {
                                //这种情况刷新一次授权码吧
                                isOtherdeniedRef = true;
                                isNeedRefToken = true;
                                if (isNeedPiiToken)
                                {
                                    isNeedRefPIIToken = true;
                                }
                                goto doRefToken;
                            }
                            //如果是授权码问题获取下数据库最新的
                            //if (details.Contains("Access to requested resource is denied") && account.TokenExpiresDate > DateTime.Now && reftokenTime == 0)
                            //{
                            //    reftokenTime++;
                            //    var dbApiUser = TokenBiz.GetNewestApiUser(account);
                            //    if (dbApiUser != null)
                            //    {
                            //        var dbAcount = AmazonAccount.From(dbApiUser);
                            //        if (dbAcount.AccessToken != account.AccessToken && dbAcount.TokenExpiresDate > account.TokenExpiresDate)
                            //        {
                            //            account.TokenExpiresDate = dbAcount.TokenExpiresDate;
                            //            account.AccessToken = dbAcount.AccessToken;
                            //            account.RefreshTokenExpiresDate = dbAcount.RefreshTokenExpiresDate;
                            //            account.RefreshToken = dbAcount.RefreshToken;
                            //        }
                            //    }
                            //}
                            errorList.Add("(" + code + ")" + msg);
                        }
                        result.Message = "亚马逊返回提示：" + string.Join(";", errorList);
                    }
                }
            }
            catch (Exception e)
            {
                result = e.ToOperateResult();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CanonicalQuerys">查询需要的</param>
        /// <param name="canonicalHeaders"></param>
        /// <param name="url"></param>
        /// <param name="urlpart"></param>
        /// <param name="Method"></param>
        /// <param name="CanonicalQueryString"></param>
        /// <returns></returns>
        public static String CalculateStringToSignV4(IDictionary<string, object> CanonicalQuerys, IDictionary<string, string> canonicalHeaders, string signedHeaders, string hexRequestPayload, string url, string urlpart, string Method)
        {
            StringBuilder data = new StringBuilder();
            IDictionary<string, object> sorted =
                  new SortedDictionary<string, object>(CanonicalQuerys, StringComparer.Ordinal);
            IDictionary<string, string> canonicalsorted =
                  new SortedDictionary<string, string>(canonicalHeaders, StringComparer.Ordinal);
            data.Append(Method);
            data.Append("\n");
            data.Append(urlpart);
            data.Append("\n");
            bool isFirst = true;
            foreach (KeyValuePair<string, object> pair in sorted)
            {
                if (pair.Value != null)
                {
                    if (!isFirst)
                    {
                        data.Append("&");
                    }
                    isFirst = false;
                    data.Append(UrlEncode(pair.Key, false));
                    data.Append("=");
                    data.Append(UrlEncode(pair.Value.ToString(), false));

                }
            }
            data.Append("\n");
            foreach (KeyValuePair<string, string> pair in canonicalsorted)
            {
                if (pair.Value != null)
                {
                    data.Append(pair.Key);
                    data.Append(":");
                    data.Append(pair.Value);
                    data.Append("\n");
                }
            }
            data.Append("\n");
            data.Append(signedHeaders);
            data.Append("\n");
            data.Append(hexRequestPayload);
            String result = data.ToString();
            return result;
        }

        public static String UrlEncode(String data, bool path)
        {
            StringBuilder encoded = new StringBuilder();
            String unreservedChars = "abcdefg***********6789-_.~" + (path ? "/" : "");

            foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    encoded.Append(symbol);
                }
                else
                {
                    encoded.Append("%" + String.Format("{0:X2}", (int)symbol));
                }
            }

            return encoded.ToString();

        }
    }
}
