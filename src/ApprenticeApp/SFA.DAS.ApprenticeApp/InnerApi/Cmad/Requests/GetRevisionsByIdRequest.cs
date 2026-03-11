using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.Cmad.Requests
{
    public class GetRevisionsByIdRequest : IGetApiRequest
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;
        private readonly long _revisionId;

        public GetRevisionsByIdRequest(Guid apprenticeId, long apprenticeshipId, long revisionId)
        {
            _apprenticeId = apprenticeId;
            _apprenticeshipId = apprenticeshipId;
            _revisionId = revisionId;
        }

        public string GetUrl => $"apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_revisionId}";
    }
}
