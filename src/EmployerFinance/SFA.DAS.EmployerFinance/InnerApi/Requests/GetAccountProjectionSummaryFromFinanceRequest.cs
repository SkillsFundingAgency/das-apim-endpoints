using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests
{
    public class GetAccountProjectionSummaryFromFinanceRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetAccountProjectionSummaryFromFinanceRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"/api/accounts/{_accountId}/projection-summary";
    }
}
