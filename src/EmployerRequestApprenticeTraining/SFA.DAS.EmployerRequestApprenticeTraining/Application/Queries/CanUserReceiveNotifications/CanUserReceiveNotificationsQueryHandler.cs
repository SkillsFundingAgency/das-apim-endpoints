using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.CanUserReceiveNotifications
{
    public class CanUserReceiveNotificationsQueryHandler : IRequestHandler<CanUserReceiveNotificationsQuery, bool>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public CanUserReceiveNotificationsQueryHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }

        public async Task<bool> Handle(CanUserReceiveNotificationsQuery query, CancellationToken cancellationToken)
        {
            var teamMembers = await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(
                       new GetAccountTeamMembersRequest(query.AccountId));

            if (teamMembers == null || !teamMembers.Any())
            {
                throw new ApiResponseException(System.Net.HttpStatusCode.NotFound, "No team members found or API returned an empty list.");
            }

            var member = teamMembers.FirstOrDefault(c =>
                c.UserRef.Equals(query.UserId.ToString(), StringComparison.OrdinalIgnoreCase)
                && c.CanReceiveNotifications);

            return member != null;
        }
    }
}