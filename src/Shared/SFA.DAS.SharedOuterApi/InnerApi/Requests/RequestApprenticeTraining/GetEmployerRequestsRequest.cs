using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class GetEmployerRequestsRequest : IGetApiRequest
    {
        public long AccountId { get; set; }

        public GetEmployerRequestsRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/employerrequest/account/{AccountId}";
    }
}
