using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Finance
{
    public class GetTransferAllowanceByAccountIdRequest : IGetApiRequest
    {
        private readonly long AccountId;

        public GetTransferAllowanceByAccountIdRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/{AccountId}/transferAllowanceByAccountId";
    }
}