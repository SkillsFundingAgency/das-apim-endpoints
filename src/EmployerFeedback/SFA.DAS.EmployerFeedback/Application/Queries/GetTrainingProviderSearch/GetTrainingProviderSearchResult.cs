using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetTrainingProviderSearch
{
    public class GetTrainingProviderSearchResult
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public List<TrainingProviderSearchResult> Providers { get; set; }
    }

    public class TrainingProviderSearchResult
    {
        public long Ukprn { get; set; }
        public string ProviderName { get; set; }
        public TrainingProviderFeedback Feedback { get; set; }
        public bool HasNewStart { get; set; }
        public bool HasActive { get; set; }
        public bool HasCompleted { get; set; }
    }

    public class TrainingProviderFeedback
    {
        public DateTime DateTimeCompleted { get; set; }
        public string ProviderRating { get; set; }
        public int FeedbackSource { get; set; }
    }
}