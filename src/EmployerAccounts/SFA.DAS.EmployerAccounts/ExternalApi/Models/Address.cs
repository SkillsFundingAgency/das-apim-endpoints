using Newtonsoft.Json;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Models
{
    public class Address
    {
        [JsonProperty("premises")]
        public string Premises { get; set; }

        [JsonProperty("address_line_1")]
        public string CompaniesHouseLine1 { get; set; }

        public string Line1 => !string.IsNullOrEmpty(Premises) ? Premises : CompaniesHouseLine1;

        [JsonProperty("address_line_2")]
        public string CompaniesHouseLine2 { get; set; }

        public string Line2 => !string.IsNullOrEmpty(Premises) ? CompaniesHouseLine1 : CompaniesHouseLine2;

        public string Line3 => !string.IsNullOrEmpty(Premises) ? CompaniesHouseLine2 : null;

        [JsonProperty("locality")]
        public string TownOrCity { get; set; }

        [JsonProperty("region")]
        public string County { get; set; }

        [JsonProperty("postal_code")]
        public string PostCode { get; set; }
    }
}
