using System;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PostWorkHistoryApiRequest : IPostApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;

        public PostWorkHistoryApiRequest(Guid applicationId, Guid candidateId, PostWorkHistoryApiRequestData data)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            Data = data;
        }

        public string PostUrl => $"candidates/{_candidateId}/applications/{_applicationId}/work-history";
        public object Data { get; set; }

        public class PostWorkHistoryApiRequestData
        {
            public WorkHistoryType WorkHistoryType { get; set; }
            public string EmployerName { get; set; }
            public string JobTitle { get; set; }
            public string JobDescription { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
    }
}
