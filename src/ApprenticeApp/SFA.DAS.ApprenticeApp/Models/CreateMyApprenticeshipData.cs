using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class CreateMyApprenticeshipData
    {
        public long Uln { get; set; }
        public long ApprenticeshipId { get; set; }
        public string EmployerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public string TrainingCode { get; set; }
        public string StandardUId { get; set; }
    }
}
