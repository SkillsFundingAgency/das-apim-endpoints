using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback
{
    public class GetLatestEmployerFeedbackResponse
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }

        public List<EmployerFeedbackItem> EmployerFeedbacks { get; set; }
    }

    public class EmployerFeedbackItem
    {
        public long Ukprn { get; set; }
        public FeedbackResultItem Result { get; set; }
    }

    public class FeedbackResultItem
    {
        public DateTime DateTimeCompleted { get; set; }

        public string ProviderRating { get; set; }

        public int FeedbackSource { get; set; }
    }
}