using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ApprovalCreatedRequest : IPostApiRequest<ApprovalCreatedRequestData>
    {
        public string PostUrl => "/approvals";

        public ApprovalCreatedRequestData Data { get; set; }
    }

    public class ApprovalCreatedRequestData
    {
        public Guid RegistrationId  { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public DeliveryModel DeliveryModel { get; set; }
        public string CourseName { get; set; }
        public int CourseLevel { get; set; }
        public int CourseDuration { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
    }
}