using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommandHandler : IRequestHandler<CreateApprenticeMemberCommand, CreateApprenticeMemberCommandResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public CreateApprenticeMemberCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<CreateApprenticeMemberCommandResult> Handle(CreateApprenticeMemberCommand request, CancellationToken cancellationToken)
    {
        return _apiClient.PostApprenticeMember(request, cancellationToken);
    }
}