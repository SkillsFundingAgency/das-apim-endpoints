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
            var result = await _apiClient.Get<GetAccountUsersResponse>(new GetAccountUsersRequest(request.AccountId));

            return new GetAccountUsersResult
            {
                UserRef = result.UserRef,
                Name = result.Name,
                Email = result.Email,
                Role = result.Role,
                CanReceiveNotifications = result.CanReceiveNotifications,
                Status = result.Status
            };
        }
    }
}