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
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services;

public interface IEmployerAccountsService
{
    Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(EmployerProfile employerProfile);
    Task<EmployerProfile> PutEmployerAccount(EmployerProfile employerProfile);
    Task<IEnumerable<TeamMember>> GetTeamMembers(long accountId);
}

public class EmployerAccountsService(
    IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> employerProfilesApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient)
    : IEmployerAccountsService
{
    public async Task<IEnumerable<TeamMember>> GetTeamMembers(long accountId)
    {
        var response = await accountsApiClient.GetAll<GetAccountTeamMembersResponse>(new GetAccountTeamMembersRequest(accountId));
        
        return response.Select(usersResponse => new TeamMember
        {
            Name = usersResponse.Name,
            Status = usersResponse.Status,
            Email = usersResponse.Email,
            Role = usersResponse.Role,
            UserRef = usersResponse.UserRef,
            CanReceiveNotifications = usersResponse.CanReceiveNotifications
        });
    }

    public async Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(EmployerProfile employerProfile)
    {
        var userId = employerProfile.UserId;
        var firstName = string.Empty;
        var lastName = string.Empty;
        var displayName = string.Empty;
        var isSuspended = false;

        var userResponse =
            await employerProfilesApiClient.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                new GetEmployerUserAccountRequest(employerProfile.UserId));

        if (userResponse.StatusCode == HttpStatusCode.NotFound)
        {
            if (!Guid.TryParse(employerProfile.UserId, out _))
            {
                var employerUserResponse =
                    await employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(
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
            // logic to check if the email address is the different/changed for the user account.
            // if true then update the EmployerAccount with latest information.
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

        var userAccountsResponse = await accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userId));

        var returnList = new ConcurrentBag<EmployerAccountUser>();

        await Parallel.ForEachAsync(userAccountsResponse, new ParallelOptions { MaxDegreeOfParallelism = 10 },
            async (userAccount, _) =>
            {
                var teamMembers =
                    await accountsApiClient.GetAll<GetAccountTeamMembersResponse>(
                        new GetAccountTeamMembersRequest(userAccount.AccountId));
                
                var member = teamMembers.FirstOrDefault(c =>
                    c.UserRef.Equals(userId, StringComparison.CurrentCultureIgnoreCase));

                if (member != null)
                {
                    returnList.Add(new EmployerAccountUser
                    {
                        Role = member.Role,
                        DasAccountName = userAccount.DasAccountName,
                        EncodedAccountId = userAccount.EncodedAccountId,
                        FirstName = firstName,
                        LastName = lastName,
                        UserId = userId,
                        DisplayName = displayName,
                        IsSuspended = isSuspended,
                        ApprenticeshipEmployerType = userAccount.ApprenticeshipEmployerType
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
                IsSuspended = isSuspended,
                ApprenticeshipEmployerType = ApprenticeshipEmployerType.Unknown
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
        var employerUserResponse = await employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(
            new PutUpsertEmployerUserAccountRequest(
                new Guid(employerProfile.UserId),
                employerProfile.GovIdentifier,
                employerProfile.Email,
                employerProfile.FirstName,
                employerProfile.LastName));

        if (employerUserResponse?.Body != null)
        {
            // external api call to update the employer_account repo with latest information.
            await accountsApiClient.PutWithResponseCode<NullResponse>(new PutAccountUserRequest(
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
        }

        return null;
    }
}