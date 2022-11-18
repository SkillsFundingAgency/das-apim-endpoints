using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetUserAccountsRequest : IGetAllApiRequest
    {
        private readonly string _userId;

        public GetUserAccountsRequest(string userId)
        {
            _userId = userId;
        }

        public string GetAllUrl => $"api/user/{_userId}/accounts";
    
    }
}