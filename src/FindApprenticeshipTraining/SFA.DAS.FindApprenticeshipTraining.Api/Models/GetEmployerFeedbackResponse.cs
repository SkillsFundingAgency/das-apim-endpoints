using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetEmployerFeedbackResponse
    {
        public int TotalEmployerResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetEmployerFeedbackItem> FeedbackDetail { get ; set ; }
        public IEnumerable<EmployerFeedbackAttributeDetail> FeedbackAttributes { get; set; }
    }

    public class GetEmployerFeedbackItem
    {
        public string FeedbackName { get; set; }
        public int FeedbackCount { get;set; }

        public static implicit operator GetEmployerFeedbackItem(GetEmployerFeedbackRatingItem source)
        {
            return new GetEmployerFeedbackItem
            {
                FeedbackCount = source.FeedbackCount,
                FeedbackName = source.FeedbackName
            };
        }
    }

    public class GetEmployerFeedbackAttributeItem
    {
        public int Weakness { get ; set ; }

        public int Strength { get ; set ; }

        public string AttributeName { get ; set ; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetEmployerFeedbackAttributeItem(InnerApi.Responses.GetEmployerFeedbackAttributeItem source)
        {
            return new GetEmployerFeedbackAttributeItem
            {
                AttributeName = source.AttributeName,
                Strength = source.Strength,
                Weakness = source.Weakness,
                Rating = source.Strength - source.Weakness,
                TotalVotes = source.Strength + source.Weakness
            };
        }
    }

    public enum EmployerFeedbackRatingType
    {
        NotYetReviewed = 0,
        VeryPoor = 1,
        Poor = 2,
        Good = 3,
        Excellent = 4
    }

    public class EmployerFeedbackAttributeDetail
    {
        public string AttributeName { get; set; }        
        public int Weakness { get; set; }
        public int Strength { get; set; }

    }

}