using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Admin.InnerApi.Requests
{
    public class GetUserActionByCodeRequest : IGetApiRequest
    {
        public string Code { get; set; }

        public GetUserActionByCodeRequest(string code)
        {
            Code = code;
        }

        public string GetUrl => $"api/users/useractions/{Code}";
    }
}
