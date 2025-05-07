using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.PostApplicationsCountApiRequest;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record PostApplicationsCountApiRequest(Guid CandidateId, PostApplicationsCountApiRequestData Payload) : IPostApiRequest
    {
        public object Data { get; set; } = Payload.Statuses;
        public string PostUrl => $"api/candidates/{CandidateId}/applications/count";

        public record PostApplicationsCountApiRequestData(List<ApplicationStatus> Statuses)
        {
            public List<ApplicationStatus> Statuses { get; set; } = Statuses;
        }
    }
}
