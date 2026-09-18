using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetCandidateByEmailAddressApiRequest(string emailAddress) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/by-email/{emailAddress}";
}