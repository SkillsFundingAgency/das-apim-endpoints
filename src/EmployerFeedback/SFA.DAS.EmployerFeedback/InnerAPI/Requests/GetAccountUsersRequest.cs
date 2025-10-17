using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.InnerApi.Requests
{
    public class GetAccountUsersRequest : IGetApiRequest
    {
        public GetAccountUsersRequest(long accountId)
        {
            AccountId = accountId;
        }

        public long AccountId { get; }

        public string GetUrl => $"api/accounts/internal/{AccountId}/users";
    }
}