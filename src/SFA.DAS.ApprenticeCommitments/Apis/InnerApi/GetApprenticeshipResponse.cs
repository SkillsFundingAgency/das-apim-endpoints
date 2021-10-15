using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ApprenticeshipResponse
    {
        public long Id { get; set; }
        public long CommitmentStatementId { get; set; }
        public string EmployerName { get; set; }
        public string TrainingProviderName { get; set; }
        public string CourseName { get; set; }
        public int CourseLevel { get; set; }
        public string CourseOption { get; set; }
        public int CourseDuration { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public bool? TrainingProviderCorrect { get; set; }
        public bool? EmployerCorrect { get; set; }
        public bool? RolesAndResponsibilitiesCorrect { get; set; }
        public bool? ApprenticeshipDetailsCorrect { get; set; }
        public bool? ApprenticeshipDeliveryConfirmation { get; set; }
        public bool? HowApprenticeshipDeliveredCorrect { get; set; }
        public DateTime? ConfirmBefore { get; set; }
        public DateTime? ConfirmedOn { get; set; }
        public bool DisplayChangeNotification { get; set; }
        public DateTime? LastViewed { get; set; }
    }
}