using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.InnerApi.Requests
{
    public class CreateApprenticeshipRequest : IPostApiRequest
    {
        public string PostUrl => "/apprenticeships";

        public object Data { get; set; }
    }

    public class CreateApprenticeshipRequestData
    {
        public Guid RequestId { get; set; }
        public long ApprenticeshipId { get; set; }
        public string Email { get; set; }
    }

    public class CreateApprenticeshipResponse
    {
    }
}