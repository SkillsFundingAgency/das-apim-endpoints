using SFA.DAS.FindAnApprenticeship.Services;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

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
        public List<Address> OtherAddresses { get; set; }

        public bool IsPrimaryLocation { get; set; }
        public TrainingProviderDetails TrainingProvider { get; set; }
        public string AdditionalQuestion1 { get; set; }
        public string AdditionalQuestion2 { get; set; }
        public bool IsDisabilityConfident { get; set; }

        [JsonPropertyName("disabilityConfident")]
        public dynamic DisabilityConfident { get; set; }
		public DateTime? ClosedDate { get; set; }
        public string City => EmployerLocation.AddressLine4;
        public string Postcode => EmployerLocation.Postcode;
        public bool IsExternalVacancy => false;
        public string ExternalVacancyUrl => string.Empty;
        public VacancyDataSource VacancySource { get; set; }

        public string AdditionalTrainingDescription { get; set; }
        public string ApplicationInstructions { get; set; }
        public string ApplicationUrl { get; set; }
        public string Description { get; set; }
        public ContactDetail EmployerContact { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LiveDate { get; set; }
        public int NumberOfPositions { get; set; }
        public string OutcomeDescription { get; set; }
        public ContactDetail ProviderContact { get; set; }
        public IEnumerable<Qualification> Qualifications { get; init; } = [];
        public string ShortDescription { get; set; }
        public IEnumerable<string> Skills { get; init; } = [];
        public DateTime StartDate { get; set; }
        public string ThingsToConsider { get; set; }
        public string TrainingDescription { get; set; }
        public VacancyLocationType VacancyLocationType { get; set; }
        public Wage Wage { get; set; }

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
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }

        public class TrainingProviderDetails
        {
            public string Name { get; set; }
            public int Ukprn { get; set; }
        }

        public class Qualification
        {
            public string QualificationType { get; set; }
            public string Subject { get; set; }
            public string Grade { get; set; }
            public int Weighting { get; set; }
        }
        
        public class ContactDetail
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }
    }
    
    public class Wage
    {
        public decimal? FixedWageYearlyAmount { get; set; }
        public int Duration { get; set; }
        public int DurationUnit { get; set; }
        public string WageAdditionalInformation { get; set; }
        public int WageType { get; set; }
        public decimal? WeeklyHours { get; set; }
        public string WorkingWeekDescription { get; set; }
    }

    public class GetClosedVacanciesByReferenceResponse
    {
        public List<GetClosedVacancyResponse> Vacancies { get; set; }
    }
}
