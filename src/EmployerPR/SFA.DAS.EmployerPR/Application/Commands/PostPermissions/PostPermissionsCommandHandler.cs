using MediatR;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Commands.PostPermissions;

public class PostPermissionsCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<PostPermissionsCommand, Unit>
{
    public async Task<Unit> Handle(PostPermissionsCommand command,
        CancellationToken cancellationToken)
    {
        if (command.Operations.Count > 0)
        {
            await _providerRelationshipsApiRestClient.PostPermissions(command, cancellationToken);
            return Unit.Value;
        }

        await _providerRelationshipsApiRestClient.RemovePermissions(command.UserRef, command.Ukprn!.Value, command.AccountLegalEntityId, cancellationToken);
        return Unit.Value;
    }
}