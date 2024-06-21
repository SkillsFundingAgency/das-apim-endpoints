using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.EmployerAccounts.ExternalApi.Models;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Responses
{
    public class SearchCharitiesResponse
    {
        [JsonPropertyName("items")]
        public IEnumerable<CompanySearchResultsItem> Companies { get; set; }
    }

    public class CompanySearchResultsItem
    {
        [JsonPropertyName("title")]
        public string CompanyName { get; set; }

        [JsonPropertyName("company_number")]
        public string CompanyNumber { get; set; }

        [JsonPropertyName("date_of_creation")]
        public DateTime? DateOfIncorporation { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("company_status")]
        public string CompanyStatus { get; set; }
    }
}
