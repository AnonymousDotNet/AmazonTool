using AmazonTool.Model;
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
    }
}
