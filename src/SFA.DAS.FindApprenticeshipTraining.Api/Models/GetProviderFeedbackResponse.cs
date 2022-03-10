using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetProviderFeedbackResponse
    {
        public int TotalEmployerResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetProviderFeedbackItem> FeedbackDetail { get ; set ; }
        
        public GetProviderFeedbackAttributes FeedbackAttributes { get; set; }
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

    public class GetProviderFeedbackAttributes
    {
        public List<StrengthDetail> Strengths { get; private set; }
        public List<WeaknessDetail> Weaknesses { get; private set; }

        public GetProviderFeedbackAttributes Build(List<GetProviderFeedbackAttributeItem> feedbackAttributeItems)
        {
            return new GetProviderFeedbackAttributes
            { 
                Strengths = feedbackAttributeItems
                .Where(c => c.Strength > 0)
                .Select(c => new StrengthDetail { StrengthName = c.AttributeName, StrengthCount = c.Strength }).ToList(),

                Weaknesses = feedbackAttributeItems
                .Where(c => c.Weakness > 0)
                .Select(c => new WeaknessDetail { WeaknessName = c.AttributeName, WeaknessCount = c.Weakness }).ToList()
            };
        }
    }

    public enum FeedbackRatingType
    {
        NotYetReviewed = 0,
        VeryPoor = 1,
        Poor = 2,
        Good = 3,
        Excellent = 4
    }

    public class StrengthDetail
    {
        public string StrengthName { get; set; }
        public int StrengthCount { get; set; }
    }

    public class WeaknessDetail
    {
        public string WeaknessName { get; set; }
        public int WeaknessCount { get; set; }
    }




}