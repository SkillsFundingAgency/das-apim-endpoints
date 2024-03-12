namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
public class GetLiveVacanciesApiResponse
{
    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}
public class LiveVacancy
{
    public string Id { get; set; } = null!;
    public Guid VacancyId { get; set; }
    public DateTime ClosingDate { get; set; }
    public string? Description { get; set; }
    public DisabilityConfident DisabilityConfident { get; set; }
    public string? EmployerContactEmail { get; set; }
    public string? EmployerContactName { get; set; }
    public string? EmployerContactPhone { get; set; }
    public string? ProviderContactEmail { get; set; }
    public string? ProviderContactName { get; set; }
    public string? ProviderContactPhone { get; set; }
    public string? EmployerDescription { get; set; }
    public Address? EmployerLocation { get; set; }
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
    public string AccountPublicHashedId { get; set; } = null!;
    public string AccountLegalEntityPublicHashedId { get; set; } = null!;
    public VacancyType? VacancyType { get; set; }
    public string ApplicationMethod { get; set; } = null!;
    public string? ApplicationUrl { get; set; }
    public string? AdditionalQuestion1 { get; set; }
    public string? AdditionalQuestion2 { get; set; }
}

public class TrainingProvider
{
    public string Name { get; set; } = null!;
    public long Ukprn { get; set; }
}

public class Address
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? Postcode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Wage
{
    public int Duration { get; set; }
    public string? DurationUnit { get; set; }
    public decimal? FixedWageYearlyAmount { get; set; }
    public string? WageAdditionalInformation { get; set; }
    public string? WageType { get; set; }
    public decimal WeeklyHours { get; set; }
    public string? WorkingWeekDescription { get; set; }
    public decimal? ApprenticeMinimumWage { get; set; }
    public decimal? Under18NationalMinimumWage { get; set; }
    public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    public decimal? Over25NationalMinimumWage { get; set; }
    public string WageText { get; set; } = null!;
}

public class Qualification
{
    public string QualificationType { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Grade { get; set; } = null!;
    public string Weighting { get; set; } = null!;
}
