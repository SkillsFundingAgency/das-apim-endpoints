using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetApprenticeFeedbackResponse
    {
        public int TotalApprenticeResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetApprenticeFeedbackItem> FeedbackDetail { get ; set ; }
        public IEnumerable<GetApprenticeFeedbackAttributeItem> FeedbackAttributes { get; set; }
    }

    public class GetApprenticeFeedbackItem
    {
        public string Rating { get; set; }
        public int Count { get;set; }

        public static implicit operator GetApprenticeFeedbackItem(GetApprenticeFeedbackRatingItem source)
        {
            return new GetApprenticeFeedbackItem
            {
                Count = source.Count,
                Rating = source.Rating
            };
        }
    }

    public class GetApprenticeFeedbackAttributeItem
    {
        public int Disagree { get ; set ; }

        public int Agree { get ; set ; }

        public string Name { get ; set ; }
        public string Category { get; set; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetApprenticeFeedbackAttributeItem(InnerApi.Responses.GetApprenticeFeedbackAttributeItem source)
        {
            return new GetApprenticeFeedbackAttributeItem
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