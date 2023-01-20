using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.InnerApi.Requests
{
    public class GetAccountBalanceRequest : IGetApiRequest
    {
        private readonly string _accountId;

        public GetAccountBalanceRequest(string accountId)
        {
            _accountId = accountId;
        }
        public string GetUrl => $"api/accounts/balances?accountIds={_accountId}";
    }
}