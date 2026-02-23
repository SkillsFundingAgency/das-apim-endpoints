using SFA.DAS.SharedOuterApi.Domain;
using DomainAddress = SFA.DAS.SharedOuterApi.Models.Address;

namespace SFA.DAS.RecruitQa.Domain;

public class Vacancy
{
    public long? VacancyReference { get; init; }
    public string? Title { get; init; }
    public DateTime? StartDate { get; init; }
    public List<DomainAddress>? EmployerLocations { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public string? EmployerName { get; init; }
    public int? NumberOfPositions { get; init; }
    public string? ProgrammeId { get; set; }
    public Wage? Wage { get; set; }
}

public class Wage
{
    public int? Duration { get; set; }
    public DurationUnit? DurationUnit { get; set; }
    public string WorkingWeekDescription { get; set; }
    public decimal? WeeklyHours { get; set; }
    public WageType? WageType { get; set; }
    public decimal? FixedWageYearlyAmount { get; set; }
    public string WageAdditionalInformation { get; set; }
    public string CompanyBenefitsInformation { get; set; }
}