using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications
{
    public class GetAccountTeamMembersWhichReceiveNotificationsQueryHandler : IRequestHandler<GetAccountTeamMembersWhichReceiveNotificationsQuery, GetAccountTeamMembersWhichReceiveNotificationsQueryResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public GetAccountTeamMembersWhichReceiveNotificationsQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetAccountTeamMembersWhichReceiveNotificationsQueryResult> Handle(GetAccountTeamMembersWhichReceiveNotificationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _accountsApiClient.Get<GetAccountTeamMembersWhichReceiveNotificationsResponse>(new GetAccountTeamMembersWhichReceiveNotificationsRequest(request.AccountId));
            return ((GetAccountTeamMembersWhichReceiveNotificationsQueryResult)(response));
        }
    }
}
