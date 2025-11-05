using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;
using System.Text.Json.Serialization;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
public class GetLiveVacanciesApiResponse
{
    [JsonProperty("items")]
    public IEnumerable<LiveVacancy> Items { get; set; } = null!;
    [JsonProperty("pageInfo")]
    public PageInfo PageInfo { get; set; } = null!;
}

public record PageInfo
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
}

public class LiveVacancy
{
    public Guid Id { get; set; }
    public Guid VacancyId { get; set; }
    public DateTime ClosingDate { get; set; }
    public string? Description { get; set; }
    public bool DisabilityConfident { get; set; }
    public string? EmployerContactEmail { get; set; }
    public string? EmployerContactName { get; set; }
    public string? EmployerContactPhone { get; set; }
    public string? ProviderContactEmail { get; set; }
    public string? ProviderContactName { get; set; }
    public string? ProviderContactPhone { get; set; }
    public string? EmployerDescription { get; set; }
    public Address? EmployerLocation { get; set; }
    public List<Address>? EmployerLocations { get; set; } = [];
    [JsonPropertyName("employerLocationOption"), System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string? EmployerLocationInformation { get; set; }

    public Address? Address
    {
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

    public List<Address>? OtherAddresses
    {
        get
        {
            if (EmployerLocationOption is not AvailableWhere.MultipleLocations) return null;
            var otherAddresses = EmployerLocations?
                .DistinctBy(x => x.ToSingleLineAddress())
                .ToList();
            return otherAddresses;
        }
    }

    public string? EmployerName { get; set; }
    public string? EmployerWebsiteUrl { get; set; }
    public bool IsAnonymous { get; set; } 
    public DateTime LiveDate { get; set; }
    public int NumberOfPositions { get; set; }
    public string? OutcomeDescription { get; set; }
    public string? ProgrammeId { get; set; }
    public IEnumerable<Qualification> Qualifications { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public IEnumerable<string> Skills { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public string? Status { get; set; }
    public string? ThingsToConsider { get; set; }
    public string Title { get; set; } = null!;
    public string? TrainingDescription { get; set; }
    public TrainingProvider TrainingProvider { get; set; } = null!;
    public long VacancyReference { get; set; }
    public Wage Wage { get; set; } = null!;
    public int AccountId { get; set; }
    public int AccountLegalEntityId { get; set; }
    public VacancyType? VacancyType { get; set; }
    public string ApplicationMethod { get; set; } = null!;
    public string? ApplicationInstructions { get; set; }
    public string? ApplicationUrl { get; set; }
    public string? AdditionalQuestion1 { get; set; }
    public string? AdditionalQuestion2 { get; set; }
    public string? AdditionalTrainingDescription { get; set; }
    public ApprenticeshipTypes? ApprenticeshipType { get; set; }
}

public class TrainingProvider
{
    public string Name { get; set; } = null!;
    public long Ukprn { get; set; }
}

public class Wage
{
    public int Duration { get; set; }
    public DurationUnit? DurationUnit { get; set; }
    public decimal? FixedWageYearlyAmount { get; set; }
    public string? WageAdditionalInformation { get; set; }
    public WageType? WageType { get; set; }
    public decimal WeeklyHours { get; set; }
    public string? WorkingWeekDescription { get; set; }
    public decimal? ApprenticeMinimumWage { get; set; }
    public decimal? Under18NationalMinimumWage { get; set; }
    public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    public decimal? Over25NationalMinimumWage { get; set; }
    public string WageText { get; set; } = null!;
    public string? CompanyBenefitsInformation { get; set; }
}

public class Qualification
{
    public string QualificationType { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Grade { get; set; } = null!;
    public QualificationWeighting? Weighting { get; set; }
}