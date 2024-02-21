using System.Net;
using SFA.DAS.EmployerAan.Application.User.Commands;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.Models;

namespace SFA.DAS.EmployerAan.Services;

public class EmployerAccountsService : IEmployerAccountsService
{
    private readonly IEmployerProfilesApiClient _employerProfilesApiClient;
    private readonly IAccountsApiClient _accountsApiClient;

    public EmployerAccountsService(IEmployerProfilesApiClient employerProfilesApiClient, IAccountsApiClient accountsApiClient)
    {
        _employerProfilesApiClient = employerProfilesApiClient;
        _accountsApiClient = accountsApiClient;
    }

    public async Task<IEnumerable<EmployerAccountUser>> GetEmployerAccounts(EmployerProfile employerProfile, CancellationToken cancellationToken)
    {
        var userId = employerProfile.UserId;
        var firstName = string.Empty;
        var lastName = string.Empty;
        var displayName = string.Empty;
        var isSuspended = false;

        var userResponse =
            await _employerProfilesApiClient.GetEmployerUserAccount(employerProfile.UserId, cancellationToken);

        //.GetWithResponseCode<EmployerProfileUsersApiResponse>(
        //new GetEmployerUserAccountRequest(employerProfile.UserId));


        if (userResponse.ResponseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            if (!Guid.TryParse(employerProfile.UserId, out _))
            {
                var employerUserResponse =
                    await _employerProfilesApiClient.PutEmployerUserAccount(Guid.NewGuid().ToString(),
                        new PutEmployerProfileRequest
                        {
                            GovIdentifier = employerProfile.UserId, // for some reason the code being updated seems to be setting govIdentifier to userId?
                            FirstName = employerProfile.FirstName,
                            LastName = employerProfile.LastName,
                            Email = employerProfile.Email
                        },
                        cancellationToken);
                // await _employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                //     new PutUpsertEmployerUserAccountRequest(Guid.NewGuid(), employerProfile.UserId,
                //         employerProfile.Email, employerProfile.FirstName, employerProfile.LastName));

                // If you look in 'PutUpsertEmployerUserAccountRequest' the employerProfile.UserId is set mto the govIdentifier???
                // public PutUpsertEmployerUserAccountRequest(Guid userId, string govIdentifier, string email, string firstName, string lastName)
                // {
                //     _userId = userId;
                //     Data = new
                //     {
                //         GovIdentifier = govIdentifier,
                //         FirstName = firstName,
                //         LastName = lastName,
                //         Email = email
                //     };
                // }

                // see https://github.com/SkillsFundingAgency/das-apim-endpoints/blob/master/src/EmployerProfiles/SFA.DAS.EmployerProfiles.Api/Controllers/AccountUsersController.cs
                //   and 'PutUpsertEmployerUserAccountRequest' in sharedouterapi
                //   
                var response = employerUserResponse.GetContent();

                userId = response.Id;
                firstName = response.FirstName;
                lastName = response.LastName;
                displayName = response.DisplayName;
                isSuspended = response.IsSuspended;
            }
        }
        else
        {
            // logic to check if the email address is the different/changed for the user account.
            // if true then update the EmployerAccount with latest information.
            if (!Guid.TryParse(employerProfile.UserId, out _) && userResponse.GetContent().Email != employerProfile.Email)
            {
                employerProfile.UserId = userResponse.GetContent().Id;
                employerProfile.FirstName = userResponse.GetContent().FirstName;
                employerProfile.LastName = userResponse.GetContent().LastName;
                employerProfile.GovIdentifier = userResponse.GetContent().GovUkIdentifier;
                await PutEmployerAccount(employerProfile, cancellationToken);
            }

            userId = userResponse.GetContent().Id;
            firstName = userResponse.GetContent().FirstName;
            lastName = userResponse.GetContent().LastName;
            displayName = userResponse.GetContent().DisplayName;
            isSuspended = userResponse.GetContent().IsSuspended;
        }

        // var result =
        //     await _accountsApiClient.GetAll<GetUserAccountsResponse>(new GetUserAccountsRequest(userId));
        var userAccounts = await _accountsApiClient.GetUserAccounts(userId, cancellationToken);
        var result = userAccounts.GetContent();

        var returnList = new List<EmployerAccountUser>();
        foreach (var account in result)
        {
            //var teamMember = await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(new GetAccountTeamMembersRequest(account.EncodedAccountId));

            var teamMembers = await _accountsApiClient.GetAccountTeamMembers(account.EncodedAccountId, cancellationToken);

            var member = teamMembers.GetContent().FirstOrDefault(c => c.UserRef.Equals(userId, StringComparison.CurrentCultureIgnoreCase));

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
        }

        if (returnList.Count == 0)
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

        return returnList;
    }

    /// <summary>
    /// Method to insert/update the user information.
    /// </summary>
    /// <param name="employerProfile">typeof EmployerProfile.</param>
    /// <param name="cancellationToken">typeof CancellationToken.</param>
    /// <returns>typeof EmployerProfile.</returns>
    public async Task<EmployerProfile> PutEmployerAccount(EmployerProfile employerProfile, CancellationToken cancellationToken)
    {
        // var employerUserResponse = await _employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(
        //     new PutUpsertEmployerUserAccountRequest(
        //         new Guid(employerProfile.UserId),
        //         employerProfile.GovIdentifier,
        //         employerProfile.Email,
        //         employerProfile.FirstName,
        //         employerProfile.LastName));

        var putEmployerProfileRequest = new PutEmployerProfileRequest
        {
            GovIdentifier = employerProfile.GovIdentifier,
            Email = employerProfile.Email,
            FirstName = employerProfile.FirstName,
            LastName = employerProfile.LastName
        };

        var employerUserResponse = await _employerProfilesApiClient.PutEmployerUserAccount(employerProfile.UserId,
            putEmployerProfileRequest, cancellationToken);

        if (employerUserResponse.ResponseMessage.IsSuccessStatusCode)   // MFCMFC this might be a created status???
        {
            // external api call to update the employer_account repo with latest information.
            // await _accountsApiClient.PutWithResponseCode<NullResponse>(new PutAccountUserRequest(
            //     employerProfile.UserId,
            //     employerProfile.Email,
            //     employerProfile.FirstName,
            //     employerProfile.LastName,
            //     employerProfile.CorrelationId));

            var userAccountRequest = new PutAccountUserRequest
            {
                UserRef = employerProfile.UserId,
                FirstName = employerProfile.FirstName,
                LastName = employerProfile.LastName,
                Email = employerProfile.Email,
                CorrelationId = employerProfile.CorrelationId
            };

            await _accountsApiClient.PutAccountUser(userAccountRequest, cancellationToken);

            return new EmployerProfile
            {
                Email = employerUserResponse.GetContent().Email,
                FirstName = employerUserResponse.GetContent().FirstName,
                LastName = employerUserResponse.GetContent().LastName,
                UserId = employerUserResponse.GetContent().GovUkIdentifier
            };
        };

        return null;
    }
}