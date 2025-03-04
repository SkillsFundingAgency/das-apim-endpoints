namespace SFA.DAS.Recruit.Api.Models;

public class PostApplicationFeedbackRequest
{
    public string Status { get; set; }
    public string CandidateFeedback { get; set; }
    public long VacancyReference { get; set; }
    public string VacancyCity { get; set; }
    public string VacancyPostcode { get; set; }
    public string VacancyLocation { get; set; }
    public string VacancyTitle { get; set; }
    public string VacancyEmployerName { get; set; }
}