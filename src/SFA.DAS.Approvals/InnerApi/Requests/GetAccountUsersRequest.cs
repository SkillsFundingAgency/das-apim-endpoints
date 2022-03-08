using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAccountUsersRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountUsersRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/internal/{AccountId}/users";
    }
}