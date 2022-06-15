using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetApprenticeFeedbackResponse
    {
        public long Ukprn { get; set; }
        public IEnumerable<GetApprenticeFeedbackAttributeItem> FeedbackAttributes { get; set; }
        public IEnumerable<GetApprenticeFeedbackRatingItem> FeedbackRatings { get; set; }
    }

    public class GetApprenticeFeedbackAttributeItem : FeedbackAttributeItemBase
    {
        public string AttributeCategory { get; set; }
        public int Agree { get; set; }
        public int Disagree { get; set; }
    }

    public class GetApprenticeFeedbackRatingItem : FeedbackRatingItemBase
    {
    }
}
