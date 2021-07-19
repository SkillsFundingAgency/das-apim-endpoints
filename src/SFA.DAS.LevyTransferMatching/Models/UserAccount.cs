namespace SFA.DAS.LevyTransferMatching.Models
{
    using Newtonsoft.Json;

    public class UserAccount
    {
        [JsonProperty("HashedAccountId")]
        public string EncodedAccountId { get; set; }
        public string DasAccountName { get; set; }
    }
}