using MediatR;
using RestEase;
using SFA.DAS.EmployerPR.Common;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.EmployerPR.InnerApi.Requests;
using SFA.DAS.EmployerPR.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandHandler(
    IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient,
    IPensionRegulatorApiClient<PensionRegulatorApiConfiguration> _pensionRegulatorApiClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient)
    : IRequestHandler<AcceptCreateAccountRequestCommand>
{
    public async Task Handle(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        GetRequestResponse permissionRequest = await GetPermissionRequestDetails(_providerRelationshipsApiRestClient, command, cancellationToken);

        PensionRegulatorOrganisation organisation = await GetPensionRegulatorOrganisation(_pensionRegulatorApiClient, permissionRequest);

        PostCreateAccountResponse createAccountResponse = await CreateEmployerAccount(_accountsApiClient, command, permissionRequest, organisation);

        await AcceptPermissionRequest(command, createAccountResponse, permissionRequest.EmployerOrganisationName!, cancellationToken);

        await SendNotifications(_providerRelationshipsApiRestClient, command, permissionRequest, createAccountResponse, cancellationToken);
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

        PensionRegulatorOrganisation organisation = tprResponse.Body.First(r => r.Status == string.Empty);
        return organisation;
    }

    private static async Task<PostCreateAccountResponse> CreateEmployerAccount(IAccountsApiClient<AccountsConfiguration> _accountsApiClient, AcceptCreateAccountRequestCommand command, GetRequestResponse permissionRequest, PensionRegulatorOrganisation organisation)
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

    private async Task SendNotifications(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient, AcceptCreateAccountRequestCommand command, GetRequestResponse permissionRequest, PostCreateAccountResponse createAccountResponse, CancellationToken cancellationToken)
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
