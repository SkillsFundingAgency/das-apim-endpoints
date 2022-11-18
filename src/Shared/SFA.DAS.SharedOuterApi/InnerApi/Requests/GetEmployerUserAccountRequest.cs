using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetEmployerUserAccountRequest : IGetApiRequest
    {
        private readonly string _id;

        public GetEmployerUserAccountRequest(string id)
        {
            _id = id;
        }

        public string GetUrl => $"api/users/govuk/?id={HttpUtility.UrlEncode(_id)}";
    }
}