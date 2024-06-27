using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    public interface IEmployerAccountsService
    {
        Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(EmployerProfile employerProfile);
        Task<EmployerProfile> PutEmployerAccount(EmployerProfile employerProfile);
    }

    public class EmployerAccountsService : IEmployerAccountsService
    {
        private readonly IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> _employerProfilesApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public EmployerAccountsService(IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> employerProfilesApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _employerProfilesApiClient = employerProfilesApiClient;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(EmployerProfile employerProfile)
        {
            var userId = employerProfile.UserId;
            var firstName = string.Empty;
            var lastName = string.Empty;
            var displayName = string.Empty;
            var isSuspended = false;

            var userResponse =
                await _employerProfilesApiClient.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                    new GetEmployerUserAccountRequest(employerProfile.UserId));

            if (userResponse.StatusCode == HttpStatusCode.NotFound)
            {
                if (!Guid.TryParse(employerProfile.UserId, out _))
                {
                    var employerUserResponse =
                        await _employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                            new PutUpsertEmployerUserAccountRequest(Guid.NewGuid(), employerProfile.UserId,
                                employerProfile.Email, employerProfile.FirstName, employerProfile.LastName));

                    userId = employerUserResponse.Body.Id;
                    firstName = employerUserResponse.Body.FirstName;
                    lastName = employerUserResponse.Body.LastName;
                    displayName = employerUserResponse.Body.DisplayName;
                    isSuspended = employerUserResponse.Body.IsSuspended;
                }
            }
            else
            {
                if (!Guid.TryParse(employerProfile.UserId, out _) && userResponse.Body.Email != employerProfile.Email)
                {
                    employerProfile.UserId = userResponse.Body.Id;
                    employerProfile.FirstName = userResponse.Body.FirstName;
                    employerProfile.LastName = userResponse.Body.LastName;
                    employerProfile.GovIdentifier = userResponse.Body.GovUkIdentifier;
                    await PutEmployerAccount(employerProfile);
                }

                userId = userResponse.Body.Id;
                firstName = userResponse.Body.FirstName;
                lastName = userResponse.Body.LastName;
                displayName = userResponse.Body.DisplayName;
                isSuspended = userResponse.Body.IsSuspended;
            }

            var result = await _accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userId));

            var returnList = new ConcurrentBag<EmployerAccountUser>();

            await Parallel.ForEachAsync(result, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                async (account, _) =>
                {
                    var teamMembers =
                        await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(
                            new GetAccountTeamMembersRequest(account.EncodedAccountId));
                    var member = teamMembers.FirstOrDefault(c =>
                        c.UserRef.Equals(userId, StringComparison.CurrentCultureIgnoreCase));

                    if (member != null)
                    {
                        returnList.Add(new EmployerAccountUser
                        {
                            Role = member.Role,
                            DasAccountName = account.DasAccountName,
                            EncodedAccountId = account.EncodedAccountId,
                            FirstName = firstName,
                            LastName = lastName,
                            UserId = userId,
                            DisplayName = displayName,
                            IsSuspended = isSuspended
                        });
                    }
                });

            if (returnList.IsEmpty)
            {
                returnList.Add(new EmployerAccountUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserId = userId,
                    DisplayName = displayName,
                    IsSuspended = isSuspended
                });
            }

            return returnList.ToList();
        }


        /// <summary>
        /// Method to insert/update the user information.
        /// </summary>
        /// <param name="employerProfile">typeof EmployerProfile.</param>
        /// <returns>typeof EmployerProfile.</returns>
        public async Task<EmployerProfile> PutEmployerAccount(EmployerProfile employerProfile)
        {
            var employerUserResponse = await _employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                new PutUpsertEmployerUserAccountRequest(
                    new Guid(employerProfile.UserId),
                    employerProfile.GovIdentifier,
                    employerProfile.Email,
                    employerProfile.FirstName,
                    employerProfile.LastName));
            
            if (employerUserResponse?.Body != null)
            {
                // external api call to update the employer_account repo with latest information.
                await _accountsApiClient.PutWithResponseCode<NullResponse>(new PutAccountUserRequest(
                    employerProfile.UserId,
                    employerProfile.Email,
                    employerProfile.FirstName,
                    employerProfile.LastName,
                    employerProfile.CorrelationId));

                return new EmployerProfile
                {
                    Email = employerUserResponse.Body.Email,
                    FirstName = employerUserResponse.Body.FirstName,
                    LastName = employerUserResponse.Body.LastName,
                    UserId = employerUserResponse.Body.GovUkIdentifier
                };
            };

            return null;
        }
    }
}