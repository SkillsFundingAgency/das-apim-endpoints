using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public class GetAccountsByUserRequest : IGetApiRequest
    {
        private readonly string _userId;

        public GetAccountsByUserRequest(string userId)
        {
            _userId = userId;
        }

        public string GetUrl => $"api/user/{_userId}/accounts";
    }
}