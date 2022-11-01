
using System.Collections.Generic;
using System.Linq;


namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetEmployerFeedbackItem
    {
        public IEnumerable<GetEmployerFeedbackAttributeItem> FeedbackAttributes { get; set; }
        public IEnumerable<GetEmployerFeedbackRatingItem> FeedbackRatings { get; set; }

        public static implicit operator GetEmployerFeedbackItem(GetEmployerFeedbackResponse item)
        {
            if (item == null)
                return null;

            return new GetEmployerFeedbackItem()
            {
                FeedbackAttributes = item.ProviderAttribute.Select(a => new GetEmployerFeedbackAttributeItem()
                {
                    AttributeName = a.Name,
                    Strength = a.Strength,
                    Weakness = a.Weakness
                }),
                FeedbackRatings = new List<GetEmployerFeedbackRatingItem>()
                {
                    new GetEmployerFeedbackRatingItem() { FeedbackName = "Stars", FeedbackCount = item.Stars },
                    new GetEmployerFeedbackRatingItem() { FeedbackName = "ReviewCount", FeedbackCount = item.ReviewCount },
                }
            };
        }

        public static implicit operator GetEmployerFeedbackItem(GetEmployerFeedbackSummaryItem item)
        {
            if (item == null)
                return null;

            return new GetEmployerFeedbackItem()
            {
                FeedbackAttributes = new List<GetEmployerFeedbackAttributeItem>(),
                FeedbackRatings = new List<GetEmployerFeedbackRatingItem>()
                {
                    new GetEmployerFeedbackRatingItem() { FeedbackName = "Stars", FeedbackCount = item.Stars },
                    new GetEmployerFeedbackRatingItem() { FeedbackName = "ReviewCount", FeedbackCount = item.ReviewCount },
                }
            };
        }
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
