using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetEmployerFeedbackItem
    {
        public IEnumerable<GetEmployerFeedbackAttributeItem> FeedbackAttributes { get; set; }
        public IEnumerable<GetEmployerFeedbackRatingItem> FeedbackRatings { get; set; }
    }

    public class GetEmployerFeedbackAttributeItem
    {
        public string AttributeName { get; set; }
        public int Weakness { get; set; }
        public int Strength { get; set; }
    }

    public class GetEmployerFeedbackRatingItem
    {
        public string FeedbackName { get; set; }
        public int FeedbackCount { get; set; }
    }
}
