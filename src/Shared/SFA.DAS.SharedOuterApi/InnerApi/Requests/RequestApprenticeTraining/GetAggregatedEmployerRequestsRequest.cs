using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class GetAggregatedEmployerRequestsRequest : IGetApiRequest
    {
        public long AccountId { get; set; }

        public GetAggregatedEmployerRequestsRequest(long accountId)
        { 
            AccountId = accountId;
        }

        public string GetUrl => $"api/employerrequest/account/{AccountId}/aggregated";
    }
}
