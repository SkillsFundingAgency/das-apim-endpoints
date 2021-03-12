using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class GetApprenticeshipRequest : IGetApiRequest
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;

        public GetApprenticeshipRequest(Guid apprenticeId, long apprenticeshipId)
        => (_apprenticeId, _apprenticeshipId) = (apprenticeId, apprenticeshipId);

        public string GetUrl => $"apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}";
    }
}