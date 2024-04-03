using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetCandidateNameApiRequest : IGetApiRequest
{
    private readonly string _govUkIdentifier;

    public GetCandidateNameApiRequest(string govUkIdentifier)
    {
        _govUkIdentifier = govUkIdentifier;
    }

    public string GetUrl => $"api/candidates/{_govUkIdentifier}";
}