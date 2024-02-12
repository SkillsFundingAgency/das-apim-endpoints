using SFA.DAS.SharedOuterApi.Interfaces;
using System;


namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class DeleteJobRequest : IDeleteApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly Guid _workHistoryId;

        public DeleteJobRequest(Guid applicationId, Guid candidateId, Guid workHistoryId)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _workHistoryId = workHistoryId;
        }

        public string DeleteUrl => $"candidates/{_candidateId}/applications/{_applicationId}/work-history/{_workHistoryId}";
    }
}
