﻿namespace SFA.DAS.ProviderFeedback.Api.Models
{
    public class GetApprenticeFeedbackResponse
    {
        public int TotalApprenticeResponses { get ; set ; }
        public int TotalFeedbackRating { get ; set ; }
        public IEnumerable<GetApprenticeFeedbackAttributeItem> FeedbackAttributes { get; set; }
    }

    public class GetApprenticeFeedbackAttributeItem
    {
        public int Disagree { get ; set ; }
        public int Agree { get ; set ; }
        public string Name { get ; set ; }
        public string Category { get; set; }
        public int TotalVotes { get ; set ; }
        public int Rating { get ; set ; }

        public static implicit operator GetApprenticeFeedbackAttributeItem(Application.InnerApi.Responses.GetApprenticeFeedbackAttributeItem source)
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