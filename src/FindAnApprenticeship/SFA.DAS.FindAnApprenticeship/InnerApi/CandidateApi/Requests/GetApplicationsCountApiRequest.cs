using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record GetApplicationsCountApiRequest(Guid CandidateId, ApplicationStatus Status) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{CandidateId}/applications/count?status={Status}";
    }
}