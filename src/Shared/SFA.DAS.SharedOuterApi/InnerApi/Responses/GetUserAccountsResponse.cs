using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetUserAccountsResponse
    {
        [JsonPropertyName("HashedAccountId")]
        public string EncodedAccountId { get; set; }
        [JsonPropertyName("DasAccountName")]
        public string DasAccountName { get; set; }
        [JsonPropertyName("Role")]
        public string Role { get; set; }
    }
}