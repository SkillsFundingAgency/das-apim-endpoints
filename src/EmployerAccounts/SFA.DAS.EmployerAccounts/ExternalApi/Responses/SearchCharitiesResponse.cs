using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.EmployerAccounts.ExternalApi.Models;


namespace SFA.DAS.EmployerAccounts.ExternalApi.Responses
{
    public class SearchCharitiesResponse
    {
        [JsonProperty("items")]
        public IEnumerable<CompanySearchResultsItem> Companies { get; set; }
    }

    public class CompanySearchResultsItem
    {
        [JsonProperty("title")]
        public string CompanyName { get; set; }

        [JsonProperty("company_number")]
        public string CompanyNumber { get; set; }

        [JsonProperty("date_of_creation")]
        public DateTime? DateOfIncorporation { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("company_status")]
        public string CompanyStatus { get; set; }
    }
}
