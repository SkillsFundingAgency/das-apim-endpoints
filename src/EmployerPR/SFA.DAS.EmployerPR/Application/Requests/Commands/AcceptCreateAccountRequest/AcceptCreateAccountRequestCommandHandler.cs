using System.Net;
using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;
using SFA.DAS.EmployerPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandHandler(
    IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient,
    IPensionRegulatorApiClient<PensionRegulatorApiConfiguration> _pensionRegulatorApiClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient,
    IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> _employerProfilesApiClient)
    : IRequestHandler<AcceptCreateAccountRequestCommand, AcceptCreateAccountRequestCommandResult>
{
    public async Task<AcceptCreateAccountRequestCommandResult> Handle(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        GetRequestResponse permissionRequest = await GetPermissionRequestDetails(_providerRelationshipsApiRestClient, command, cancellationToken);

        PensionRegulatorOrganisation organisation = await GetPensionRegulatorOrganisation(_pensionRegulatorApiClient, permissionRequest);

        await UpdateUserProfile(command);

        PostCreateAccountResponse createAccountResponse = await CreateEmployerAccount(command, permissionRequest, organisation);

        await AcceptPermissionRequest(command, createAccountResponse, permissionRequest.EmployerOrganisationName!, cancellationToken);

        await SendNotifications(command, permissionRequest, createAccountResponse, cancellationToken);

        return new AcceptCreateAccountRequestCommandResult(createAccountResponse.AccountId);
    }

    private static async Task<GetRequestResponse> GetPermissionRequestDetails(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient, AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Response<GetRequestResponse?> getRequestResponse = await _providerRelationshipsApiRestClient.GetRequest(command.RequestId, cancellationToken);

        GetRequestResponse permissionRequest = getRequestResponse.GetContent()!;
        return permissionRequest;
    }

    private static async Task<PensionRegulatorOrganisation> GetPensionRegulatorOrganisation(IPensionRegulatorApiClient<PensionRegulatorApiConfiguration> _pensionRegulatorApiClient, GetRequestResponse permissionRequest)
    {
        GetPensionsRegulatorOrganisationsRequest tprRequest = new(permissionRequest.EmployerAORN, permissionRequest.EmployerPAYE);
        var tprResponse = await _pensionRegulatorApiClient.GetWithResponseCode<IEnumerable<PensionRegulatorOrganisation>>(tprRequest);

        PensionRegulatorOrganisation organisation = tprResponse.Body.First(r => r.Status.Equals(TprOrganisationStatus.NotClosed, StringComparison.OrdinalIgnoreCase));
        return organisation;
    }

    private async Task UpdateUserProfile(AcceptCreateAccountRequestCommand command)
    {
        var userResponse = await _employerProfilesApiClient.GetWithResponseCode<EmployerProfileUsersApiResponse>(new GetEmployerUserAccountRequest(command.UserRef.ToString()));
        var govId = userResponse.StatusCode == HttpStatusCode.NotFound ? command.UserRef.ToString() : userResponse.Body.GovUkIdentifier;

        PutUpsertEmployerUserAccountRequest request = new(command.UserRef, govId, command.Email, command.FirstName, command.LastName);
        var response = await _employerProfilesApiClient.PutWithResponseCode<EmployerProfileUsersApiResponse>(request);
        response.EnsureSuccessStatusCode();
    }

    private async Task<PostCreateAccountResponse> CreateEmployerAccount(AcceptCreateAccountRequestCommand command, GetRequestResponse permissionRequest, PensionRegulatorOrganisation organisation)
    {
        CreateAccountRequestBody createAccountRequestBody = new()
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            EmployerAddress = organisation.Address.ToString(),
            EmployerAorn = permissionRequest.EmployerAORN,
            EmployerPaye = permissionRequest.EmployerPAYE,
            EmployerOrganisationReferenceNumber = organisation.UniqueIdentity.ToString(),
            EmployerOrganisationName = permissionRequest.EmployerOrganisationName,
            RequestId = command.RequestId.ToString(),
            UserRef = command.UserRef,
        };
        PostCreateAccountRequest postCreateAccountRequest = new() { Data = createAccountRequestBody };

        var accountsApiResponse = await _accountsApiClient.PostWithResponseCode<CreateAccountRequestBody, PostCreateAccountResponse>(postCreateAccountRequest);
        accountsApiResponse.EnsureSuccessStatusCode();
        PostCreateAccountResponse createAccountResponse = accountsApiResponse.Body;
        return createAccountResponse;
    }

    private async Task AcceptPermissionRequest(AcceptCreateAccountRequestCommand command, PostCreateAccountResponse createAccountResponse, string employerName, CancellationToken cancellationToken)
    {
        AcceptCreateAccountRequestBody acceptCreateAccountRequest = new()
        {
            ActionedBy = command.UserRef.ToString(),
            AccountDetails = new(createAccountResponse.AccountId, employerName),
            AccountLegalEntityDetails = new(createAccountResponse.AccountLegalEntityId, employerName)
        };
        await _providerRelationshipsApiRestClient.AcceptCreateAccountRequest(command.RequestId, acceptCreateAccountRequest, cancellationToken);
    }

    private async Task SendNotifications(AcceptCreateAccountRequestCommand command, GetRequestResponse permissionRequest, PostCreateAccountResponse createAccountResponse, CancellationToken cancellationToken)
    {
        PostNotificationsRequest notificationsRequest = new([
            new NotificationModel()
            {
                TemplateName = PermissionEmailTemplateType.WelcomeConfirmed.ToString(),
                NotificationType = NotificationType.Employer.ToString(),
                Ukprn = permissionRequest.Ukprn,
                EmailAddress = command.Email,
                Contact = $"{command.FirstName} {command.LastName}",
                AccountLegalEntityId = createAccountResponse.AccountLegalEntityId,
                CreatedBy = command.UserRef.ToString()
            },
            new NotificationModel()
            {
                TemplateName = PermissionEmailTemplateType.CreateAccountAccepted.ToString(),
                NotificationType = NotificationType.Provider.ToString(),
                Ukprn = permissionRequest.Ukprn,
                AccountLegalEntityId = createAccountResponse.AccountLegalEntityId,
                CreatedBy = command.UserRef.ToString()
            }
        ]);

        await _providerRelationshipsApiRestClient.PostNotifications(notificationsRequest, cancellationToken);
    }
}
