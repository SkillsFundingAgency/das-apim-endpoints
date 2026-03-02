using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests
{
    [ExcludeFromCodeCoverage]
    public class ConfirmApprenticeshipPatchRequest : IPatchApiRequest<object>
    {
        private readonly Guid _apprenticeId;
        private readonly long _apprenticeshipId;
        private readonly long _revisionId;

        public ConfirmApprenticeshipPatchRequest(Guid apprenticeId, long apprenticeshipId, long revisionId)
        {
            _apprenticeId = apprenticeId;
            _apprenticeshipId = apprenticeshipId;
            _revisionId = revisionId;
        }

        public string PatchUrl => $"apprentices/{_apprenticeId}/apprenticeships/{_apprenticeshipId}/revisions/{_revisionId}/confirmations";
        public object Data { get; set; }
    }
}
