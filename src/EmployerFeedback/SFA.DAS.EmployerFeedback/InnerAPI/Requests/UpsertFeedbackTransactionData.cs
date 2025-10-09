using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class UpsertFeedbackTransactionData
    {
        public List<ApprenticeshipStatusItem> Active { get; set; } = new List<ApprenticeshipStatusItem>();
        public List<ApprenticeshipStatusItem> Completed { get; set; } = new List<ApprenticeshipStatusItem>();
        public List<ApprenticeshipStatusItem> NewStart { get; set; } = new List<ApprenticeshipStatusItem>();
    }

    public class ApprenticeshipStatusItem
    {
        public long Ukprn { get; set; }
        public string CourseCode { get; set; }
    }
}