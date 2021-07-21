using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class ChangeApprenticeshipRequest : IPostApiRequest<ChangeApprenticeshipRequestData>
    {
        public string PostUrl => "/apprenticeships/change";

        public ChangeApprenticeshipRequestData Data { get; set; }
    }

    public class ChangeApprenticeshipRequestData
    {
        public long? CommitmentsContinuedApprenticeshipId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public string CourseName { get; set; }
        public int CourseLevel { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
    }
}