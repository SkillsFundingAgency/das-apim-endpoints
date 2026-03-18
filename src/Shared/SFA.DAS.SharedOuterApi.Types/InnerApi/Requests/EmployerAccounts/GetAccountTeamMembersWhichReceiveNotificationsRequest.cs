using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts
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
