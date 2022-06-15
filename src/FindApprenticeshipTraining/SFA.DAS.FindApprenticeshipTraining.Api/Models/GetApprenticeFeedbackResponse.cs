﻿using System.Collections.Generic;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Models
{
    public class GetApprenticeFeedbackResponse
    {
        public int TotalApprenticeResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetApprenticeFeedbackItem> FeedbackDetail { get ; set ; }
        public IEnumerable<ApprenticeFeedbackAttributeDetail> FeedbackAttributes { get; set; }
    }

    public class GetApprenticeFeedbackItem
    {
        public string FeedbackName { get; set; }
        public int FeedbackCount { get;set; }

        public static implicit operator GetApprenticeFeedbackItem(GetApprenticeFeedbackRatingItem source)
        {
            return new GetApprenticeFeedbackItem
            {
                FeedbackCount = source.FeedbackCount,
                FeedbackName = source.FeedbackName
            };
        }
    }

    public class GetApprenticeFeedbackAttributeItem
    {
        public int Disagree { get ; set ; }

        public int Agree { get ; set ; }

        public string AttributeName { get ; set ; }
        public string Category { get; set; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetApprenticeFeedbackAttributeItem(InnerApi.Responses.GetApprenticeFeedbackAttributeItem source)
        {
            return new GetApprenticeFeedbackAttributeItem
            {
                AttributeName = source.AttributeName,
                Agree = source.Agree,
                Disagree = source.Disagree,
                Rating = source.Agree - source.Disagree,
                TotalVotes = source.Agree + source.Disagree
            };
        }
    }

    public enum ApprenticeFeedbackRatingType
    {
        NotYetReviewed = 0,
        VeryPoor = 1,
        Poor = 2,
        Good = 3,
        Excellent = 4
    }

    public class ApprenticeFeedbackAttributeDetail
    {
        public string AttributeName { get; set; }        
        public string AttributeCategory { get; set; }
        public int Disagree { get; set; }
        public int Agree { get; set; }

    }

}