using SFA.DAS.FindAnApprenticeship.Services;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses
{
    public class GetClosedVacancyResponse: IVacancy
    {
        [JsonPropertyName("vacancyReference")]
        public long VacancyReferenceNumeric { get; set; }
        [JsonIgnore]
        public string VacancyReference => $"VAC{VacancyReferenceNumeric}";
        public string EmployerName { get; set; }
        public string Title { get; set; }
        public DateTime ClosingDate { get; set; }
        public string ProgrammeId { get; set; }
        public int CourseId => Convert.ToInt32(ProgrammeId);
        public Address EmployerLocation { get; set; }
        public TrainingProviderDetails TrainingProvider { get; set; }
        public string AdditionalQuestion1 { get; set; }
        public string AdditionalQuestion2 { get; set; }
        [JsonIgnore]
        public bool IsDisabilityConfident => DisabilityConfident == "Yes";
        public string DisabilityConfident { get; set; }
		public DateTime? ClosedDate { get; set; }

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

        public class TrainingProviderDetails
        {
            public string Name { get; set; }
            public int Ukprn { get; set; }
        }
    }

    public class GetClosedVacanciesByReferenceResponse
    {
        public List<GetClosedVacancyResponse> Vacancies { get; set; }
    }
}
