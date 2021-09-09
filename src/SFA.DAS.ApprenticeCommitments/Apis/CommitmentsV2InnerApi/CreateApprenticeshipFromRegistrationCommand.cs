using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi
{
    public class CreateApprenticeshipFromRegistration
    {
        public Guid RegistrationId { get; set; }
        public Guid ApprenticeId { get; set; }
    }
}