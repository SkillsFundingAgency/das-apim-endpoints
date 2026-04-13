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
    public class GetUserAccountsQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        : IRequestHandler<GetUserAccountsQuery, GetUserAccountsQueryResult>
    {
        public async Task<GetUserAccountsQueryResult> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.GetAll<GetAccountsByUserResponse>(new GetAccountsByUserRequest(request.UserId));

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