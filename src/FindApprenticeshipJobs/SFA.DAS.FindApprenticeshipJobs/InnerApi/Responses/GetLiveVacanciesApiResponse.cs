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
    public Guid VacancyId { get; set; }
    public string? ApplicationInstructions { get; set; }
    public string? ApplicationMethod { get; set; }
    public string? ApplicationUrl { get; set; }
    public DateTime ApprovedDate { get; set; }
    public DateTime ClosedDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public string? ClosureReason { get; set; } // not in recruit - always null in response
    public User? CreatedByUser { get; set; } // not in recruit - always null in response
    public DateTime CreatedDate { get; set; }
    public User? DeletedByUser { get; set; } // not in recruit - always null in response
    public DateTime DeletedDate { get; set; }
    public string? Description { get; set; }
    public DisabilityConfident DisabilityConfident { get; set; }
    public string? EmployerAccountId { get; set; } // not in recruit - always null in response
    public EmployerContact? EmployerContact { get; set; }
    public string? EmployerDescription { get; set; }
    public Address? EmployerLocation { get; set; }
    public string? EmployerName { get; set; }
    public string? EmployerNameOption { get; set; } // not in recruit - always null in response
    public string? EmployerWebsiteUrl { get; set; }
    public bool IsAnonymous { get; set; } 
    public string? GeoCodeMethod { get; set; } // not in recruit - always null in response
    public bool? IsDeleted { get; set; }
    public User? LastUpdatedByUser { get; set; } // not in recruit - always null in response
    public DateTime LastUpdatedDate { get; set; }
    public string? LegalEntityName { get; set; } // not in recruit - always null in response
    public DateTime LiveDate { get; set; }
    public int NumberOfPositions { get; set; }
    public string? OutcomeDescription { get; set; }
    public string? OwnerType { get; set; } // not in recruit - always null in response
    public string? ProgrammeId { get; set; }
    public string? ProgrammeLevel { get; set; }
    public string? ProgrammeType { get; set; }
    public IEnumerable<Qualification>? Qualifications { get; set; }
    public string? ShortDescription { get; set; }
    public IEnumerable<string>? Skills { get; set; }
    public DateTime StartDate { get; set; }
    public string? Status { get; set; } // not in recruit - always null in response
    public User? SubmittedByUser { get; set; } // not in recruit - always null in response
    public DateTime SubmittedDate { get; set; }
    public string? ThingsToConsider { get; set; }
    public string? Title { get; set; }
    public string? TrainingDescription { get; set; }
    public TrainingProvider? TrainingProvider { get; set; }
    public long VacancyReference { get; set; }
    public Wage? Wage { get; set; }
    public int? EducationLevelNumber { get; set; }
    public string? AccountPublicHashedId { get; set; }
    public string? AccountLegalEntityPublicHashedId { get; set; }
    public int? RouteId { get; set; } // in recruit and NOT in db - always null in response
    public string? WorkExperience { get; set; } // in recruit and NOT in db - always null in response
    public VacancyType? VacancyType { get; set; }
    public string? AdditionalQuestion1 { get; set; }
    public string? AdditionalQuestion2 { get; set; }
    public string? Id { get; set; }
    public string? ViewType { get; set; }
    public DateTime? LastUpdated { get; set; }

}

public class User
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public Guid UserId { get; set; }
}

public class TrainingProvider
{
    public string? Name { get; set; }
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
}

public class Qualification
{
    public string? QualificationType { get; set; }
    public string? Subject { get; set; }
    public string? Grade { get; set; }
    public string? Weighting { get; set; }
}

public class EmployerContact 
{
    public string? EmployerContactEmail { get; set; }
    public string? EmployerContactName { get; set; }
    public string? EmployerContactPhone { get; set; }
}

