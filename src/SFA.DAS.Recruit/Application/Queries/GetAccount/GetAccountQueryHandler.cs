using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetAccount
{
    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, GetAccountQueryResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

        public GetAccountQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetAccountQueryResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAccountByIdResponse>(new GetAccountByIdRequest(request.AccountId));

            if (result == null)
            {
                return new GetAccountQueryResult();
            }
            
            return new GetAccountQueryResult
            {
                AccountId = result.AccountId,
                HashedAccountId = result.HashedAccountId
            };
        }
    }
}