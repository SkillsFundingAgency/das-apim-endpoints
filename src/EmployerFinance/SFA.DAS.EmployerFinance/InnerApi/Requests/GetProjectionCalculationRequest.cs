using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests
{
    public class GetProjectionCalculationRequest : IGetApiRequest
    {
        private readonly long _accountId;

        public GetProjectionCalculationRequest(long accountId)
        {
            _accountId = accountId;
        }

        public string GetUrl => $"/api/accounts/{_accountId}/AccountProjection/projected-summary";
    }
}
