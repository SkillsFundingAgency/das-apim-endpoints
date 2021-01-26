using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class CreateApprenticeshipRequest : IPostApiRequest
    {
        public string PostUrl => "/apprenticeships";

        public object Data { get; set; }
    }

    public class CreateApprenticeshipRequestData
    {
        public Guid RegistrationId  { get; set; }
        public long ApprenticeshipId { get; set; }
        public string Email { get; set; }
    }

    public class CreateApprenticeshipResponse
    {
    }
}