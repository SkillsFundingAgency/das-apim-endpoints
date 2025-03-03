namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetEmployerFeedbackAnnualResponse
    {
        public IEnumerable<GetEmployerFeedbackAnnualSummary> AnnualEmployerFeedbackDetails { get; set; }
    }
    public class GetEmployerFeedbackAnnualSummary
    {
        public int TotalEmployerResponses { get; set; }
        public int TotalFeedbackRating { get; set; }
        public string TimePeriod { get; set; }
        public IEnumerable<GetEmployerFeedbackAnnualAttributeItem> FeedbackAttributes { get; set; }
    }

    public class GetEmployerFeedbackAnnualAttributeItem
    {
        public int Weakness { get ; set ; }
        public int Strength { get ; set ; }
        public string AttributeName { get ; set ; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetEmployerFeedbackAnnualAttributeItem(Application.InnerApi.Responses.GetEmployerFeedbackAnnualAttributeItem source)
        {
            return new GetEmployerFeedbackAnnualAttributeItem
            {
                AttributeName = source.Name,
                Strength = source.Strength,
                Weakness = source.Weakness,
                Rating = source.Strength - source.Weakness,
                TotalVotes = source.Strength + source.Weakness
            };
        }
    }
}