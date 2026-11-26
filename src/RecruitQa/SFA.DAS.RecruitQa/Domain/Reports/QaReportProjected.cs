namespace SFA.DAS.RecruitQa.Domain.Reports;

public class QaReportProjected
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
    public string? CourseName { get; set; }
    public string? CourseLevel { get; set; }
    public List<string> ReferredFields { get; set; } = [];
    public string? ReviewedBy { get; set; }
    public string? ReviewerComment { get; set; }

    public static QaReportProjected FromQaReport(QaReport qaReport, string? courseName, string? courseLevel)
    {
        return new QaReportProjected
        {
            VacancyTitle = qaReport.VacancyTitle,
            VacancyReference = qaReport.VacancyReference,
            SubmissionNumber = qaReport.SubmissionNumber,
            DateSubmitted = qaReport.DateSubmitted,
            SlaDeadline = qaReport.SlaDeadline,
            ReviewStarted = qaReport.ReviewStarted,
            ReviewCompleted = qaReport.ReviewCompleted,
            Outcome = qaReport.Outcome,
            SlaExceededByHours = qaReport.SlaExceededByHours,
            TimeTakenToReview = qaReport.TimeTakenToReview,
            NumberOfIssuesReported = qaReport.NumberOfIssuesReported,
            VacancySubmittedBy = qaReport.VacancySubmittedBy,
            VacancySubmittedByUser = qaReport.VacancySubmittedByUser,
            Employer = qaReport.Employer,
            DisplayName = qaReport.DisplayName,
            TrainingProvider = qaReport.TrainingProvider,
            VacancyPostcode = qaReport.VacancyPostcode,
            CourseName = courseName,
            CourseLevel = courseLevel,
            ReferredFields = qaReport.ReferredFields,
            ReviewedBy = qaReport.ReviewedBy,
            ReviewerComment = qaReport.ReviewerComment,
        };
    }
}