using MediatR;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Application.Admins.Commands.Create;
public class CreateAdminMemberCommandHandler : IRequestHandler<CreateAdminMemberCommand, CreateAdminMemberCommandResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public CreateAdminMemberCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<CreateAdminMemberCommandResult> Handle(CreateAdminMemberCommand command,
        CancellationToken cancellationToken)
    {
        return _apiClient.CreateAdminMember(command, cancellationToken);
    }
}