namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetApprenticeFeedbackAnnualResponse
    {
        public IEnumerable<GetApprenticeFeedbackAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
    }
    public class GetApprenticeFeedbackAnnualSummary
    {
        public int TotalApprenticeResponses { get; set; }
        public int TotalFeedbackRating { get; set; }
        public string TimePeriod { get; set; }
        public IEnumerable<GetApprenticeFeedbackAnnualAttributeItem> FeedbackAttributes { get; set; }
    }
    public class GetApprenticeFeedbackAnnualAttributeItem
    {
        public int Disagree { get; set; }
        public int Agree { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int TotalVotes { get; set; }
        public int Rating { get; set; }

        public static implicit operator GetApprenticeFeedbackAnnualAttributeItem(Application.InnerApi.Responses.GetApprenticeFeedbackAnnualAttributeItem source)
        {
            return new GetApprenticeFeedbackAnnualAttributeItem
            {
                Name = source.Name,
                Category = source.Category,
                Agree = source.Agree,
                Disagree = source.Disagree,
                Rating = source.Agree - source.Disagree,
                TotalVotes = source.Agree + source.Disagree
            };
        }
    }
}