using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerFinance.Application.Queries.AccountUsers.Queries
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, GetAccountsQueryResult>
    {
        private readonly IEmployerAccountsService _employerAccountService;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public GetAccountsQueryHandler(IEmployerAccountsService employerAccountService,IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _employerAccountService = employerAccountService;
            _accountsApiClient = accountsApiClient;
        }
        public async Task<GetAccountsQueryResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var employerAccounts = (await _employerAccountService.GetEmployerAccounts(new EmployerProfile
            {
                Email = request.Email,
                UserId = request.UserId
            })).ToList();
            
            var userAccountResponse = employerAccounts.Select(async (c)=>
            {
                var user =  new AccountUser
                {
                    DasAccountName = c.DasAccountName,
                    EncodedAccountId = c.EncodedAccountId,
                    Role = c.Role,
                    MinimumSignedAgreementVersion = await GetSignedAgreementVersion(c.EncodedAccountId)
                };
                return user;
            }).ToList();
            
            await Task.WhenAll(userAccountResponse);
            
            return new GetAccountsQueryResult
            {
                EmployerUserId = employerAccounts.FirstOrDefault()?.UserId,
                FirstName = employerAccounts.FirstOrDefault()?.FirstName,
                LastName = employerAccounts.FirstOrDefault()?.LastName,
                IsSuspended = employerAccounts.FirstOrDefault()?.IsSuspended ?? false,
                UserAccountResponse = userAccountResponse.Select(c=>c.Result)
            };
        }
        private async Task<int> GetSignedAgreementVersion(string encodedAccountId)
        {
            var result = await _accountsApiClient.GetWithResponseCode<GetSignedAgreementVersionResponse>(new GetSignedAgreementVersionRequest(encodedAccountId));

            return result.Body.MinimumSignedAgreementVersion;
        }
    }
}