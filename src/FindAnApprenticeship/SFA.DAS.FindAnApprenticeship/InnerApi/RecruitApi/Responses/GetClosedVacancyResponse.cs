using SFA.DAS.FindAnApprenticeship.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;

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
    public int CourseId => Int32.TryParse(ProgrammeId, out var result) ? result : -1;
    public Address EmployerLocation { get; set; }
    public List<Address>? EmployerLocations { get; set; }
    public Address? Address {
        get
        {
            return EmployerLocationOption switch
            {
                AvailableWhere.OneLocation or AvailableWhere.MultipleLocations => EmployerLocations?.FirstOrDefault(),
                AvailableWhere.AcrossEngland => null,
                _ => EmployerLocation
            };
        }
    }

    public List<Address>? OtherAddresses {
        get
        {
            if (EmployerLocationOption is not AvailableWhere.MultipleLocations)
            {
                return null;
            }

            var firstAddress = EmployerLocations!.FirstOrDefault().ToSingleLineAddress();
            var otherAddresses = EmployerLocations
                .Skip(1)
                .DistinctBy(x => x.ToSingleLineAddress())
                .Where(x => x.ToSingleLineAddress() != firstAddress)
                .ToList();
                
            return otherAddresses;
        }
    }

    [JsonPropertyName("employmentLocationInformation")]
    public string? EmploymentLocationInformation { get; set; }

    [JsonPropertyName("employerLocationOption"), JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
    public AvailableWhere? EmployerLocationOption { get; set; }
    public TrainingProviderDetails TrainingProvider { get; set; }
    public string AdditionalQuestion1 { get; set; }
    public string AdditionalQuestion2 { get; set; }
    public bool IsDisabilityConfident { get; set; }

    [JsonPropertyName("disabilityConfident")]
    public dynamic DisabilityConfident { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string City => Address.GetCity();
    public string Postcode => Address?.Postcode;
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
    public ApprenticeshipTypes ApprenticeshipType { get; init; }

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
        public QualificationWeighting? Weighting { get; set; }
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
    public DurationUnit? DurationUnit { get; set; }
    public string WageAdditionalInformation { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WageType? WageType { get; set; }
    public decimal? WeeklyHours { get; set; }
    public string WorkingWeekDescription { get; set; }
}