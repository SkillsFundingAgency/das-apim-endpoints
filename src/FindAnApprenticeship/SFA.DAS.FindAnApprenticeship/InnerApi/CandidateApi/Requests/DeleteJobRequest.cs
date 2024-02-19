using SFA.DAS.SharedOuterApi.Interfaces;
using System;


namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class DeleteJobRequest(Guid applicationId, Guid candidateId, Guid jobId) : IDeleteApiRequest
    {
        public string DeleteUrl => $"candidates/{candidateId}/applications/{applicationId}/work-history/{jobId}";
    }
}
