using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class GetAuthenticateUserRequest: IGetApiRequest
    {
        private readonly string _email;
        private readonly string _password;

        public GetAuthenticateUserRequest(string email, string password)
        {
            _email = HttpUtility.UrlEncode(email);
            _password = HttpUtility.UrlEncode(password);
        }

        public string GetUrl => $"api/users/authenticate?email={_email}&password={_password}";
    }
}