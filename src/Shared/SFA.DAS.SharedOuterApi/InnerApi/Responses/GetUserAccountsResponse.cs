using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
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

        [JsonPropertyName("EmployerType")]
        public ApprenticeshipEmployerType ApprenticeshipEmployerType { get; set; }

        [JsonPropertyName("AccountId")]
        public long AccountId { get; set; }
    }
}