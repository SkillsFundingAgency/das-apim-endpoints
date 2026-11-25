using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class LearnerNotificationStatus
    {
        public byte? StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}