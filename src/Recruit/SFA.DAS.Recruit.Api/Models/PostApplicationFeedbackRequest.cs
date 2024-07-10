namespace SFA.DAS.Recruit.Api.Models;

public class PostApplicationFeedbackRequest
{
    public string Status { get; set; }
    public string CandidateFeedback { get; set; }
    public long VacancyReference { get; set; }
}