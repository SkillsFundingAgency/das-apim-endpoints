using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetApplicationByIdApiRequest(Guid ApplicationId, Guid CandidateId)
    : IGetApiRequest
{
    public string GetUrl => $"api/candidates/{CandidateId}/applications/{ApplicationId}?includeDetail=true";
}