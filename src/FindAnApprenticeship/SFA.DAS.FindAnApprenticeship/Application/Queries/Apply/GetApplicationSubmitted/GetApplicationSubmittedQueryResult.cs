using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQueryResult
{
    public string VacancyTitle { get; set; }
    public string EmployerName { get; set; }
    public bool HasAnsweredEqualityQuestions { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime? ClosedDate { get; set; }
}
