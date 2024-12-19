using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public class GetCandidatesByActivityApiResponse
    {
        public List<Candidate> Candidates { get; set; } = [];

        public class Candidate
        {
            [JsonProperty("address")]
            public Address Address { get; set; }
            [JsonProperty("govUkIdentifier")]
            public string? GovUkIdentifier { get; set; }
            [JsonProperty("lastName")]
            public string? LastName { get; set; }
            [JsonProperty("firstName")]
            public string? FirstName { get; set; }
            [JsonProperty("middleNames")]
            public string? MiddleNames { get; set; }
            [JsonProperty("phoneNumber")]
            public string? PhoneNumber { get; set; }
            [JsonProperty("dateOfBirth")]
            public DateTime DateOfBirth { get; set; }
            [JsonProperty("createdOn")]
            public DateTime CreatedOn { get; set; }
            [JsonProperty("updatedOn")]
            public DateTime UpdatedOn { get; set; }
            [JsonProperty("termsOfUseAcceptedOn")]
            public DateTime TermsOfUseAcceptedOn { get; set; }
            [JsonProperty("status")]
            public CandidateStatus Status { get; set; }
            [JsonProperty("migratedEmail")]
            public string? MigratedEmail { get; set; }
            [JsonProperty("email")]
            public string? Email { get; set; }
            [JsonProperty("id")]
            public Guid Id { get; set; }
            [JsonProperty("migratedCandidateId")]
            public Guid? MigratedCandidateId { get; set; }
        }

        public class Address
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }
            [JsonProperty("uprn")]
            public string? Uprn { get; set; }
            [JsonProperty("addressLine1")]
            public string? AddressLine1 { get; set; }
            [JsonProperty("addressLine2")]
            public string? AddressLine2 { get; set; }
            [JsonProperty("town")]
            public string? Town { get; set; }
            [JsonProperty("county")]
            public string? County { get; set; }
            [JsonProperty("postcode")]
            public string? Postcode { get; set; }
            [JsonProperty("latitude")]
            public double Latitude { get; set; }
            [JsonProperty("longitude")]
            public double Longitude { get; set; }
            [JsonProperty("source")]
            public Guid CandidateId { get; set; }
        }
    }
}