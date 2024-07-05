using MediatR;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Application.Commands.PostPermissions;

public class PostPermissionsCommandHandler(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : IRequestHandler<PostPermissionsCommand, PostPermissionsCommandResult>
{
    public async Task<PostPermissionsCommandResult> Handle(PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsApiRestClient.PostPermissions(command, cancellationToken);
    }
}