using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests
{
    public class GetLegacyValidateCredentialsApiRequest(string email, string password) : IGetApiRequest
    {
        public string GetUrl => $"api/user/validate-credentials?email={HttpUtility.UrlEncode(email)}&password={HttpUtility.UrlEncode(password)}";
    }
}
