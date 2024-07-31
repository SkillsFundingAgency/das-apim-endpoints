using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeKsbProgressData
    {
        public Guid ApprenticeshipId { get; set; }
        public int KSBProgressType { get; set; }
        public Guid KSBId { get; set; }
        public string KsbKey { get; set; }
        public int CurrentStatus { get; set; }
        public string Note { get; set; }
    }
}
