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
        // Currently, Ukprn and CourseCode are not used, but these fields will be required later 
        // for Short Courses functionality when it is implemented.

        public long Ukprn { get; set; }
        public string CourseCode { get; set; }
    }
}