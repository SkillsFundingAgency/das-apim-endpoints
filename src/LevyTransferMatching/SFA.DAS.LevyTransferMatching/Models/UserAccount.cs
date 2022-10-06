using System.Text.Json.Serialization;

namespace SFA.DAS.LevyTransferMatching.Models
{
    public class UserAccount
    {
        [JsonPropertyName("HashedAccountId")]
        public string EncodedAccountId { get; set; }
        public string DasAccountName { get; set; }
    }
}