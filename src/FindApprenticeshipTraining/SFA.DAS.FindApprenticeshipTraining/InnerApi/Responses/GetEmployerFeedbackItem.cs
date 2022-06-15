using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetEmployerFeedbackItem
    {
        public IEnumerable<GetEmployerFeedbackAttributeItem> FeedbackAttributes { get; set; }
        public IEnumerable<GetEmployerFeedbackRatingItem> FeedbackRatings { get; set; }
    }

    public class GetEmployerFeedbackAttributeItem : FeedbackAttributeItemBase
    {
        public int Weakness { get; set; }
        public int Strength { get; set; }
    }

    public class GetEmployerFeedbackRatingItem : FeedbackRatingItemBase
    {
    }
}
