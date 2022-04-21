using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery
{
    public class GetAccountUsersQueryHandler : IRequestHandler<GetAccountUsersQuery, GetAccountUsersResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

        public GetAccountUsersQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetAccountUsersResult> Handle(GetAccountUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAccountUsersResponse>(new GetAccountUsersRequest(request.HashedAccountId));

            if (result == null)
                return null;

            return new GetAccountUsersResult(request.HashedAccountId, result.Select(x => new TeamMember
            {
                UserRef = x.UserRef,
                Name = x.Name,
                Email = x.Email,
                Role = x.Role,
                CanReceiveNotifications = x.CanReceiveNotifications,
                Status = x.Status
            }));
        }
    }
}