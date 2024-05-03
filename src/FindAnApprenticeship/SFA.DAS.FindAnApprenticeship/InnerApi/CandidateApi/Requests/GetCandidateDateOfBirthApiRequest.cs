using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
public class GetCandidateDateOfBirthApiRequest : IGetApiRequest
{
    private readonly string _govUkIdentifier;

    public GetCandidateDateOfBirthApiRequest(string govUkIdentifier)
    {
        _govUkIdentifier = govUkIdentifier;
    }

    public string GetUrl => $"api/candidates/{_govUkIdentifier}";
}
