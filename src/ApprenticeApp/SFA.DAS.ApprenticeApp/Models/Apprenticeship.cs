using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class Apprenticeship
    {
        public long Id { get; set; }
        public Guid ApprenticeId { get; set; }
        public string EmployerName { get; set; }
        public string CourseName { get; set; }
        public DateTime? ConfirmedOn { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime? LastViewed { get; set; }
        public DateTime? StoppedReceivedOn { get; set; }
        public bool IsStopped { get; set; }
        public bool HasBeenConfirmedAtLeastOnce { get; set; }
    }
}
