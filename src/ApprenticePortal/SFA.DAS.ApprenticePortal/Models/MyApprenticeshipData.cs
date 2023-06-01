using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    public class MyApprenticeship : MyApprenticeshipData
    {
        public string Title { get; set; }
    }

    public class MyApprenticeshipData
    {
        public long ApprenticeshipId { get; set; }
        public string Uln { get; set; }
        public string EmployerName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public string TrainingCode { get; set; }
        public string StandardUId { get; set; }
    }
}
