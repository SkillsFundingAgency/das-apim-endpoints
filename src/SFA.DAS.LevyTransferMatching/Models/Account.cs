using Newtonsoft.Json;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Account
    {
        [JsonProperty("HashedAccountId")]
        public string EncodedAccountId { get; set; }
        public long AccountId { get; set; }
        public string DasAccountName { get; set; }
        public decimal RemainingTransferAllowance { get; set; }
    }
}