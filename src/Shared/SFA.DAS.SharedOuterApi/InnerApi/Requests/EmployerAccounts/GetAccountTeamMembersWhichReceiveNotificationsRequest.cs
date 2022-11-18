using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAccountTeamMembersWhichReceiveNotificationsRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public GetAccountTeamMembersWhichReceiveNotificationsRequest(long accountId)
        {
            AccountId = accountId;
        }

        public string GetUrl => $"api/accounts/internal/{AccountId}/users/which-receive-notifications";
    }
}
