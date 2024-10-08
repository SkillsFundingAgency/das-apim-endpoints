using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests
{
    public class GetAggregatedEmployerRequestsRequest : IGetApiRequest
    {
        public long AccountId { get; set; }

        public GetAggregatedEmployerRequestsRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}/employer-requests/aggregated";
    }
}
