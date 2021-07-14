using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ApprenticeshipRevisions
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public DateTime LastViewed { get; set; }

        public List<ApprenticeshipRevision> Revisions { get; set; }
            = new List<ApprenticeshipRevision>();
    }

    public class ApprenticeshipRevision
    {
        public long RevisionId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime ApprovedOn { get; set; }

        public string EmployerName { get; set; } = null!;
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int CourseLevel { get; set; }
        public string? CourseOption { get; set; } = null!;
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public int DurationInMonths { get; set; }

        public bool? TrainingProviderCorrect { get; set; }
        public bool? EmployerCorrect { get; set; }
        public bool? RolesAndResponsibilitiesCorrect { get; set; }
        public bool? ApprenticeshipDetailsCorrect { get; set; }
        public bool? HowApprenticeshipDeliveredCorrect { get; set; }
        public DateTime ConfirmBefore { get; set; }
        public DateTime? ConfirmedOn { get; set; }

        public DateTime? LastViewed { get; set; }
    }
}