using System.Collections.Generic;

namespace SFA.DAS.RecruitJobs.Domain;

public record VacancyReview
{
    public Guid Id { get; init; }
    public string VacancyReference { get; init; }
    public string VacancyTitle { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime SlaDeadLine { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public ReviewStatus Status { get; init; }
    public byte SubmissionCount { get; init; }
    public string? ReviewedByUserEmail { get; init; }
    public string SubmittedByUserEmail { get; init; }
    public DateTime? ClosedDate { get; init; }
    public string? ManualOutcome { get; init; }
    public string? ManualQaComment { get; init; }
    public List<string> ManualQaFieldIndicators { get; init; }
    public string? AutomatedQaOutcome { get; init; }
    public string? AutomatedQaOutcomeIndicators { get; init; }
    public List<string> DismissedAutomatedQaOutcomeIndicators { get; init; }
    public List<string> UpdatedFieldIdentifiers { get; init; }
    public string VacancySnapshot { get; init; }
    public long AccountId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public long Ukprn { get; set; }
    public OwnerType OwnerType { get; set; }
}