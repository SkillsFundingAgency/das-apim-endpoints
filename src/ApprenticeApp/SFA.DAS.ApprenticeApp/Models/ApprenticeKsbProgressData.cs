using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeKsbProgressData
    {
        public long ApprenticeshipId { get; set; }
        public KSBProgressType? KSBProgressType { get; set; }
        public Guid KSBId { get; set; }
        public string KsbKey { get; set; }
        public KSBStatus? CurrentStatus { get; set; }
        public string Note { get; set; }
        public List<ApprenticeTask>? Tasks { get; set; }
    }

    [Flags]
    public enum KSBProgressType
    {
        Knowledge = 0,
        Skill = 1,
        Behaviour = 2
    }

    [Flags]
    public enum KSBStatus
    {
        NotStarted = 0,
        InProgress = 1,
        ReadyForReview = 2,
        Completed = 3
    }
}
