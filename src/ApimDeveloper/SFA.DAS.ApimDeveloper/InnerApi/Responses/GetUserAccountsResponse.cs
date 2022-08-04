using System.Text.Json.Serialization;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
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