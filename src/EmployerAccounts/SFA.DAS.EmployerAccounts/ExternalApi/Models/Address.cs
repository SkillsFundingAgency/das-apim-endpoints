using System.Text.Json.Serialization;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Models
{
    public class Address
    {
        [JsonPropertyName("premises")]
        public string Premises { get; set; }

        [JsonPropertyName("address_line_1")]
        public string CompaniesHouseLine1 { get; set; }

        public string Line1 => !string.IsNullOrEmpty(Premises) ? Premises : CompaniesHouseLine1;

        [JsonPropertyName("address_line_2")]
        public string CompaniesHouseLine2 { get; set; }

        public string Line2 => !string.IsNullOrEmpty(Premises) ? CompaniesHouseLine1 : CompaniesHouseLine2;

        public string Line3 => !string.IsNullOrEmpty(Premises) ? CompaniesHouseLine2 : null;

        [JsonPropertyName("locality")]
        public string TownOrCity { get; set; }

        [JsonPropertyName("region")]
        public string County { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostCode { get; set; }
    }
}
