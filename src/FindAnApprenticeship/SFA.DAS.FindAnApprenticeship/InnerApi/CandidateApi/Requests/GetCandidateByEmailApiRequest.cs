using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetCandidateByEmailApiRequest(string email) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/email/{HttpUtility.UrlEncode(email)}";
}