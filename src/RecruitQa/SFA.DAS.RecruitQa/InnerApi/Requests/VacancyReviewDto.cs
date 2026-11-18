using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class VacancyReviewDto
{
    public required long VacancyReference { get; init; }
    public required string VacancyTitle { get; init; }
    public required DateTime? CreatedDate { get; init; }
    public required DateTime? SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public required string Status { get; set; }
    public required int? SubmissionCount { get; init; }
    public string? ReviewedByUserEmail { get; init; }
    public required string SubmittedByUserEmail { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string? ManualOutcome { get; set; }
    public string? ManualQaComment { get; init; }
    public required ICollection<string> ManualQaFieldIndicators { get; init; }
    public required ICollection<ManualQaEditFieldIndicator> ManualQaEditFieldIndicators { get; init; }
    public string? AutomatedQaOutcome { get; init; }
    public ICollection<RuleOutcome> AutomatedQaOutcomeIndicators { get; init; } = [];
    public required ICollection<string> DismissedAutomatedQaOutcomeIndicators { get; init; }
    public required ICollection<string> UpdatedFieldIdentifiers { get; init; }
    public required string VacancySnapshot { get; init; }
    public int Ukprn { get; set; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public OwnerType OwnerType { get; set; }
    public string HashedAccountId { get; set; }
    public string EmployerName { get; set; }
    public AvailableWhere EmployerLocationOption { get; set; }
    public List<Address> EmployerLocations { get; set; }
    public Guid VacancyId { get; set; }
}