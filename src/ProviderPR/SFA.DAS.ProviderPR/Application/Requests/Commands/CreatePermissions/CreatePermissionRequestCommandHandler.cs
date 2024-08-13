using MediatR;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse;

namespace SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;

public class CreatePermissionRequestCommandHandler(
    IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient,
    IAccountsApiClient<AccountsConfiguration> _accountsApiClient
) : IRequestHandler<CreatePermissionRequestCommand, CreatePermissionRequestCommandResult>
{
    public async Task<CreatePermissionRequestCommandResult> Handle(CreatePermissionRequestCommand command, CancellationToken cancellationToken)
    {
        var createPermissionsResponse = await _providerRelationshipsApiRestClient.CreatePermissionsRequest(command, cancellationToken);

        var teamMembers = await _accountsApiClient.GetAll<TeamMember>(new GetAccountTeamMembersByInternalAccountIdRequest(command.AccountId));

        if (!teamMembers.Any())
        {
            return new AddAccountRequestCommandResult(createPermissionsResponse.RequestId);
        }
    }
}
