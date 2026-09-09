using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    public class Apprenticeship
    {
        public long Id { get; set; }
        public Guid ApprenticeId { get; set; }
        public string EmployerName { get; set; }
        public string TrainingProviderName { get; set; }
        public string CourseName { get; set; }
        public int CourseLevel { get; set; }
        public int ApprenticeshipType { get; set; }
        public DateTime? ConfirmedOn { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime? LastViewed { get; set; }
        public DateTime? StoppedReceivedOn { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public bool IsStopped { get; set; }
        public bool HasBeenConfirmedAtLeastOnce { get; set; }
    }
}
