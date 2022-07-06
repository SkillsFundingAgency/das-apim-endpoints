using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetUserAccounts
{
    public class GetUserAccountsQueryHandler : IRequestHandler<GetUserAccountsQuery, GetUserAccountsQueryResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

        public GetUserAccountsQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetUserAccountsQueryResult> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetAll<GetAccountsByUserResponse>(new GetAccountsByUserRequest(request.UserId));

            if (response == null)
            {
                return new GetUserAccountsQueryResult();
            }

            return new GetUserAccountsQueryResult
            {
                HashedAccountIds = response.Select(c=>c.HashedAccountId).ToList()
            };
        }
    }
}