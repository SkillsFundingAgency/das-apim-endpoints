namespace SFA.DAS.RecruitQa.InnerApi.Responses;

public class GetVacancyReviewResponse
{
    public Guid Id { get; init; }
    public long VacancyReference { get; init; }
    public required string VacancyTitle { get; init; }
    public required DateTime CreatedDate { get; init; }
    public required DateTime SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public required string Status { get; init; }
    public byte SubmissionCount { get; init; }
    public string ReviewedByUserEmail { get; init; }
    public required string SubmittedByUserEmail { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string ManualOutcome { get; set; }
    public string ManualQaComment { get; init; }
    public required List<string> ManualQaFieldIndicators { get; init; }
    public string AutomatedQaOutcome { get; set; }
    public string AutomatedQaOutcomeIndicators { get; init; }
    public required List<string> DismissedAutomatedQaOutcomeIndicators { get; init; }
    public required List<string> UpdatedFieldIdentifiers { get; init; }
    public required string VacancySnapshot { get; set; }
    public string OwnerType { get; set; }
    public Guid VacancyId { get; set; }
}