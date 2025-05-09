using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public record GetEmploymentLocationsApiRequest(Guid CandidateId, Guid ApplicationId) : IGetApiRequest
    {
        public string GetUrl => $"api/candidates/{CandidateId}/applications/{ApplicationId}/employment-locations";
    }
}