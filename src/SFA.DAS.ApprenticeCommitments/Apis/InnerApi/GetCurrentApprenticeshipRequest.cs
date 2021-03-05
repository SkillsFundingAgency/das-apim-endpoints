using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class GetCurrentApprenticeshipRequest : IGetApiRequest
    {
        private readonly Guid _apprenticeId;

        public GetCurrentApprenticeshipRequest(Guid apprenticeId)
            => _apprenticeId = apprenticeId;

        public string GetUrl => $"apprentices/{_apprenticeId}/currentapprenticeship";
    }
}