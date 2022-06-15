using Newtonsoft.Json;

namespace SFA.DAS.ApimDeveloper.InnerApi.Responses
{
    public class GetUserAccountsResponse
    {
        [JsonProperty("HashedAccountId")]
        public string EncodedAccountId { get; set; }
        [JsonProperty("DasAccountName")]
        public string DasAccountName { get; set; }
        [JsonProperty("Role")]
        public string Role { get; set; }
    }
}