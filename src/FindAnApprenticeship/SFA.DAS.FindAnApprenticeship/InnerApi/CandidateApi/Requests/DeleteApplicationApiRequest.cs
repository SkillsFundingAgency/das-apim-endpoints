using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public record DeleteApplicationApiRequest(Guid CandidateId, Guid ApplicationId): IDeleteApiRequest
{
    public string DeleteUrl => $"api/candidates/{CandidateId}/applications/{ApplicationId}";
}