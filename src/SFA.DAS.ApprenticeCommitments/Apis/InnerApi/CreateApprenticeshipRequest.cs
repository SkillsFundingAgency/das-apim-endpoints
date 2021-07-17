using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class CreateApprenticeshipRequest : IPostApiRequest<CreateApprenticeshipRequestData>
    {
        public string PostUrl => "/apprenticeships";

        public CreateApprenticeshipRequestData Data { get; set; }
    }

    public class CreateApprenticeshipRequestData
    {
        public Guid ApprenticeId  { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public string CourseName { get; set; }
        public int CourseLevel { get; set; }
        public int CourseDuration { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
    }
}