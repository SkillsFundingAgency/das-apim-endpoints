namespace SFA.DAS.RecruitQa.Domain.Reports;

public record QaReport
{
    public string? VacancyTitle { get; set; }
    public long VacancyReference { get; set; }
    public int SubmissionNumber { get; set; }
    public DateTime? DateSubmitted { get; set; }
    public DateTime? SlaDeadline { get; set; }
    public DateTime? ReviewStarted { get; set; }
    public DateTime? ReviewCompleted { get; set; }
    public string? Outcome { get; set; }
    public string SlaExceededByHours { get; set; } = "";
    public string TimeTakenToReview { get; set; } = "";
    public int NumberOfIssuesReported { get; set; }
    public string? VacancySubmittedBy { get; set; }
    public string? VacancySubmittedByUser { get; set; }
    public string? Employer { get; set; }
    public string? DisplayName { get; set; }
    public string? TrainingProvider { get; set; }
    public string? VacancyPostcode { get; set; }
    public string? ProgrammeId { get; set; }
    public List<string> ReferredFields { get; set; } = [];
    public string? ReviewedBy { get; set; }
    public string? ReviewerComment { get; set; }
}