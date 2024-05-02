using System;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses
{
    public class GetClosedVacancyResponse
    {
        public string EmployerName { get; set; }
        public string Title { get; set; }
        public DateTime ClosingDate { get; set; }
        public string ProgrammeId { get; set; }
        public Address EmployerLocation { get; set; }

        public class Address
        {
            [JsonPropertyName("addressLine1")]
            public string AddressLine1 { get; set; }
            [JsonPropertyName("addressLine2")]
            public string AddressLine2 { get; set; }
            [JsonPropertyName("addressLine3")]
            public string AddressLine3 { get; set; }
            [JsonPropertyName("addressLine4")]
            public string AddressLine4 { get; set; }
            [JsonPropertyName("postcode")]
            public string Postcode { get; set; }
        }

    }
}
