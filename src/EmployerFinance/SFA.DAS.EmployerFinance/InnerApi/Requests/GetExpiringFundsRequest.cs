using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests
{
    public class GetExpiringFundsRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetExpiringFundsRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"/api/accounts/{_accountId}/AccountProjection/expiring-funds";
    }
}
