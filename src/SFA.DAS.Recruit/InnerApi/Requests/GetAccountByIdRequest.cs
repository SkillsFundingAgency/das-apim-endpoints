using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public class GetAccountByIdRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetAccountByIdRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"api/accounts/internal/{_accountId}";
    }
}