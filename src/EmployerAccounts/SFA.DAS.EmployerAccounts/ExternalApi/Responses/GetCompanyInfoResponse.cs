using System;
using System.Text.Json.Serialization;
using SFA.DAS.EmployerAccounts.ExternalApi.Models;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Responses
{
    public class GetCompanyInfoResponse
    {
        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        [JsonPropertyName("company_number")]
        public string CompanyNumber { get; set; }

        [JsonPropertyName("date_of_creation")]
        public DateTime? DateOfIncorporation { get; set; }

        [JsonPropertyName("registered_office_address")]
        public Address RegisteredAddress { get; set; }

        [JsonPropertyName("company_status")]
        public string CompanyStatus { get; set; }
    }
}
