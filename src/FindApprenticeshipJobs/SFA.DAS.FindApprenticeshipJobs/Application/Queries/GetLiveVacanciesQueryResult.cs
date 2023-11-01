namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;
public class GetLiveVacanciesQueryResult
{
    public IEnumerable<LiveVacancy> Vacancies { get; set; }
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }
}

public class LiveVacancy
{
    public Guid VacancyId { get; set; }
    public string? ApplicationInstructions { get; set; }
    public string? ApplicationMethod { get; set; }
    public string? ApplicationUrl { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public string? ClosureReason { get; set; }
    public User CreatedByUser { get; set; }
    public DateTime CreatedDate { get; set; }
    public User DeletedByUser { get; set; }
    public DateTime? DeletedDate { get; set; }
    public string? Description { get; set; }
    public string DisabilityConfident { get; set; }
    public string EmployerAccountId { get; set; }
    //public Type EmployerContact { get; set; } - this is an object but don't know what fields should be within the object?
    public string? EmployerDescription { get; set; }
    public EmployerLocation EmployerLocation { get; set; }
    public string EmployerName { get; set; }
    public string? EmployerNameOption { get; set; }
    public string? EmployerWebsiteUrl { get; set; }
    public string? GeoCodeMethod { get; set; }
    public bool IsDeleted { get; set; }
    public User LastUpdatedByUser { get; set; }
    public DateTime LastUpdatedDate { get; set; }
    public string LegalEntityName { get; set; }
    public DateTime? LiveDate { get; set; }
    public int NumberOfPositions { get; set; }
    public string? OutcomeDescription { get; set; }
    public string? OwnerType { get; set; }
    public string ProgrammeId { get; set; }
    public IEnumerable<Qualification> Qualifications { get; set; }
    public string ShortDescription { get; set; }
    public IEnumerable<string> Skills { get; set; }
    public string SourceOrigin { get; set; }
    public string SourceType { get; set; }
    public DateTime StartDate { get; set; }
    public string Status { get; set; }
    public User SubmittedByUser { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public string? ThingsToConsider { get; set; }
    public string Title { get; set; }
    public string? TrainingDescription { get; set; }
    public TrainingProvider TrainingProvider { get; set; }
    public long VacancyReference { get; set; }
    public Wage Wage { get; set; }

}

public class User
{
    public string Email { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
}

public class EmployerLocation
{
    public string AddressLine4 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine1 { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Postcode { get; set; }
}

public class TrainingProvider
{
    public Address Address { get; set; }
    public string Name { get; set; }
    public long Ukprn { get; set; }
}

public class Address
{
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? Postcode { get; set; }

}

public class Wage
{
    public int Duration { get; set; }
    public string DurationUnit { get; set; } // months or years
    public string? FixedWageYearlyAmount { get; set; }
    public string? WageAdditionalInformation { get; set; }
    public string WageType { get; set; } //fixed wage OR nmw OR nmw for apprentices
    public string WeeklyHours { get; set; }
    public string WorkingWeekDescription { get; set; }
}

public class Qualification
{
    public string QualificationType { get; set; }
    public string Subject { get; set; }
    public string Grade { get; set; }
    public string Weighting { get; set; }
}