using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.InnerApi.Requests
{
    public class GetUserRequest : IGetApiRequest
    {
        private readonly string _email;

        public GetUserRequest(string email)
        {
            _email = email;
        }

        public string GetUrl => $"api/users?email={HttpUtility.UrlEncode(_email)}";
    }
}