using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, GetAccountsQueryResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public GetAccountsQueryHandler (IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _accountsApiClient = accountsApiClient;
        }
        public async Task<GetAccountsQueryResult> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var result =
                await _accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(request.UserId));

            
            var returnList = new List<AccountUser>();
            foreach(var account in result)
            {
                var teamMember = await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(new GetAccountTeamMembersRequest(account.EncodedAccountId));
                
                var member = teamMember.FirstOrDefault(c=>c.UserRef.Equals(request.UserId, StringComparison.CurrentCultureIgnoreCase));
                
                if(member != null)
                {
                    returnList.Add(new AccountUser
                    {
                        Role = member.Role,
                        DasAccountName = account.DasAccountName,
                        EncodedAccountId = account.EncodedAccountId
                    });
                }
            }
            
            
            return new GetAccountsQueryResult
            {
                UserAccountResponse = returnList
            };
        }
    }
}