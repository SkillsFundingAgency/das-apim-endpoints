using System.Text.Json.Serialization;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class Account
    {
        [JsonPropertyName("HashedAccountId")]
        public string EncodedAccountId { get; set; }
        public string DasAccountName { get; set; }
        public decimal RemainingTransferAllowance { get; set; }
        public string OwnerEmail { get; set; }
    }
}