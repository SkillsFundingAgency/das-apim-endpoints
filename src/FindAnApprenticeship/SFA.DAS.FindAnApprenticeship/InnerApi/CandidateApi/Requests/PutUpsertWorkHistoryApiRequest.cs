using System;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class PutUpsertWorkHistoryApiRequest : IPutApiRequest
    {
        private readonly Guid _applicationId;
        private readonly Guid _candidateId;
        private readonly Guid _id;

        public PutUpsertWorkHistoryApiRequest(Guid applicationId, Guid candidateId, Guid id, PutUpsertWorkHistoryApiRequestData data)
        {
            _applicationId = applicationId;
            _candidateId = candidateId;
            _id = id;
            Data = data;
        }

        public string PutUrl => $"candidates/{_candidateId}/applications/{_applicationId}/work-history/{_id}";
        public object Data { get; set; }

        public class PutUpsertWorkHistoryApiRequestData
        {
            public WorkHistoryType WorkHistoryType { get; set; }
            public string Employer { get; set; }
            public string JobTitle { get; set; }
            public string Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
    }
}
