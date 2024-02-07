using SFA.DAS.SharedOuterApi.Interfaces;
using System;


namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PostDeleteJobRequest(Guid applicationId, Guid candidateId, PostDeleteJobRequest.PostDeleteJobRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"candidates/{candidateId}/applications/{applicationId}/work-history/delete";
        public object Data { get; set; } = body;

        public class PostDeleteJobRequestData
        {
            public Guid JobId { get; set; }
        }
    }
}
