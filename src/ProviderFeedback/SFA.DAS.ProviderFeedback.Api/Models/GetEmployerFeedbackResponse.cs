namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetEmployerFeedbackResponse
    {
        public int TotalEmployerResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetEmployerFeedbackAttributeItem> FeedbackAttributes { get; set; }
    }
    public class GetEmployerFeedbackAttributeItem
    {
        public int Weakness { get ; set ; }

        public int Strength { get ; set ; }

        public string AttributeName { get ; set ; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetEmployerFeedbackAttributeItem(Application.InnerApi.Responses.GetEmployerFeedbackAttributeItem source)
        {
            return new GetEmployerFeedbackAttributeItem
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