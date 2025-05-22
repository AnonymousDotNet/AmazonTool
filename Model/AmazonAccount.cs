using AmazonTool.Enum;

namespace AmazonTool.Model
{
    /// <summary>
    /// Amazon Account Info
    /// </summary>
    public class AmazonAccount
    {
        /// <summary>
        /// 
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public AccountStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AWSAccessKeyId { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string MarketplaceId { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string MWSAuthToken { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string SellerId { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Creator { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? TokenExpiresDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? RefreshTokenExpiresDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TrestrictedDataToken { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? TrestrictedDataTokenExpiresDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public long? UserHashCode { get; set; }
    }
}
