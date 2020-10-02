namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetProviderFeedbackResponse
    {
        public int TotalEmployerResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
    }

    public enum FeedbackRatingType
    {
        NotYetReviewed = 0,
        VeryPoor = 1,
        Poor = 2,
        Good = 3,
        Excellent = 4
    }
}