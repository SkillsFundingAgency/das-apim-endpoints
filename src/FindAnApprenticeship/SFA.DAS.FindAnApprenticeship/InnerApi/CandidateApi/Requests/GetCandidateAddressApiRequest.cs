using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public record GetCandidateAddressApiRequest : IGetApiRequest
{
    private readonly Guid _candidateId;

    public GetCandidateAddressApiRequest(Guid candidateId)
    {
        _candidateId = candidateId;
    }

    public string GetUrl =>
        $"/api/candidates/{_candidateId}/addresses";
}