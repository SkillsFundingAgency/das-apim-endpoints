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
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? FirstViewedOn { get; set; }
        public DateTime? SignUpReminderSentOn { get; set; }
        public Apprenticeship? Apprenticeship { get; set; }
        public DateTime? StoppedReceivedOn { get; set; }
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
        public int Level { get; set; }
        public string? Option { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public int CourseDuration { get; set; }
    }

    public class RplDetails
    {
        public bool? RecognisePriorLearning { get; }
        public short? DurationReducedByHours { get; }
        public short? DurationReducedBy { get; }
    }
}
