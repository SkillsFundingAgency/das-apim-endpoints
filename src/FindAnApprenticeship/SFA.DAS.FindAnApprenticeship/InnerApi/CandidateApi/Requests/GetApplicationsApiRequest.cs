using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetApplicationsApiRequest : IGetApiRequest
{
    private readonly Guid _candidateId;

    public GetApplicationsApiRequest(Guid candidateId)
    {
        _candidateId = candidateId;
    }

    public string GetUrl => $"api/candidates/{_candidateId}/applications";
}