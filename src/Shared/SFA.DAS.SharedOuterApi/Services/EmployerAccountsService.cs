using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    public interface IEmployerAccountsService
    {
        Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(string userId, string email);
    }
    
    public class EmployerAccountsService : IEmployerAccountsService
    {
        private readonly IEmployerUsersApiClient<EmployerUsersApiConfiguration> _employerUsersApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public EmployerAccountsService(IEmployerUsersApiClient<EmployerUsersApiConfiguration> employerUsersApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _employerUsersApiClient = employerUsersApiClient;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(string userId, string email)
        {
            
            if (!Guid.TryParse(userId, out _))
            {
                var userResponse =
                    await _employerUsersApiClient.GetWithResponseCode<EmployerUsersApiResponse>(
                        new GetEmployerUserAccountRequest(userId));

                if (userResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    var employerUserResponse =
                        await _employerUsersApiClient.PutWithResponseCode<EmployerUsersApiResponse>(
                            new PutUpsertEmployerUserAccountRequest(userId, email, "", ""));

                    userId = employerUserResponse.Body.Id;    
                }
                else
                {
                    userId = userResponse.Body.Id;
                }
                
            }
            
            var result =
                await _accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userId));

            
            var returnList = new List<EmployerAccountUser>();
            foreach(var account in result)
            {
                var teamMember = await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(new GetAccountTeamMembersRequest(account.EncodedAccountId));
                
                var member = teamMember.FirstOrDefault(c=>c.UserRef.Equals(userId, StringComparison.CurrentCultureIgnoreCase));
                
                if(member != null)
                {
                    returnList.Add(new EmployerAccountUser
                    {
                        Role = member.Role,
                        DasAccountName = account.DasAccountName,
                        EncodedAccountId = account.EncodedAccountId
                    });
                }
            }

            return returnList;
        }
    }
}