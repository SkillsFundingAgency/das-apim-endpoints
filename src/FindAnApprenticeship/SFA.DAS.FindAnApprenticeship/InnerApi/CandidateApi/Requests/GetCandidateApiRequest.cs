using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public record GetCandidateApiRequest : IGetApiRequest
{
    private readonly string _govIdentifier;

    public GetCandidateApiRequest(string govIdentifier)
    {
        _govIdentifier = govIdentifier;
    }

    public string GetUrl =>
        $"/api/candidates/{_govIdentifier}";
}