using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetProviderFeedbackResponse
    {
        public int TotalEmployerResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetProviderFeedbackItem> FeedbackDetail { get ; set ; }
    }

    public class GetProviderFeedbackItem
    {
        public string FeedbackName { get; set; }
        public int FeedbackCount { get;set; }

        public static implicit operator GetProviderFeedbackItem(GetFeedbackRatingItem source)
        {
            return new GetProviderFeedbackItem
            {
                FeedbackCount = source.FeedbackCount,
                FeedbackName = source.FeedbackName
            };
        }
    }
    public class GetProviderFeedbackAttributeItem
    {
        public int Weakness { get ; set ; }

        public int Strength { get ; set ; }

        public string AttributeName { get ; set ; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetProviderFeedbackAttributeItem(GetFeedbackAttributeItem source)
        {
            return new GetProviderFeedbackAttributeItem
            {
                AttributeName = source.AttributeName,
                Strength = source.Strength,
                Weakness = source.Weakness,
                Rating = source.Strength - source.Weakness,
                TotalVotes = source.Strength + source.Weakness
            };
        }
    }
}