using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
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
