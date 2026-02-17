using System;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class Registration
    {
        public Guid RegistrationId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Email Email { get; set; }
        public Guid? ApprenticeId { get; set; }
        public Approvals Approval { get; set; }
        public DateTime? CreatedOn { get; private set; } = DateTime.UtcNow;
        public DateTime? FirstViewedOn { get; private set; }
        public DateTime? SignUpReminderSentOn { get; private set; }
        public Apprenticeship? Apprenticeship { get; private set; }
        public DateTime? StoppedReceivedOn { get; private set; }
    }

    public class  Approvals
    {
        public Approvals(
            long employerAccountLegalEntityId,
        string employerName,
        long trainingProviderId,
        string trainingProviderName,
        DeliveryModel deliveryModel,
        CourseDetails course,
        RplDetails rpl,
        int? apprenticeshipType)
        {
            EmployerAccountLegalEntityId = employerAccountLegalEntityId;
            EmployerName = employerName;
            TrainingProviderId = trainingProviderId;
            TrainingProviderName = trainingProviderName;
            DeliveryModel = deliveryModel;
            Course = course;
            Rpl = rpl;
            ApprenticeshipType = apprenticeshipType;
        }
        public long EmployerAccountLegalEntityId { get; }
        public string EmployerName { get; }
        public long TrainingProviderId { get; }
        public string TrainingProviderName { get; }
        public DeliveryModel DeliveryModel { get; }
        public CourseDetails Course { get; }
        public RplDetails Rpl { get; }
        public int? ApprenticeshipType { get; }
    }

    public class CourseDetails
    {
        public string Name { get; }
        public int Level { get; private set; }
        public string? Option { get; private set; }
        public DateTime PlannedStartDate { get; private set; }
        public DateTime PlannedEndDate { get; private set; }
        public DateTime? EmploymentEndDate { get; private set; }
        public int CourseDuration { get; private set; }
    }

    public class RplDetails
    {
        public bool? RecognisePriorLearning { get; }
        public short? DurationReducedByHours { get; }
        public short? DurationReducedBy { get; }
    }
}
