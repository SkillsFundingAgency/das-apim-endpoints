using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Employer.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommandHandler : IRequestHandler<CreateEmployerMemberCommand, CreateEmployerMemberCommandResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public CreateEmployerMemberCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<CreateEmployerMemberCommandResult> Handle(CreateEmployerMemberCommand request, CancellationToken cancellationToken)
    {
        return _apiClient.PostEmployerMember(request, cancellationToken);
    }
}
