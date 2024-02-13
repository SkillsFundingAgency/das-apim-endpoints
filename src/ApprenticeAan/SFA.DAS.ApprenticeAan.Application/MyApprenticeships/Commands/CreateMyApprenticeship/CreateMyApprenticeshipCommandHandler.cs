using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;

public class CreateMyApprenticeshipCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand, string>
{
    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;

    public CreateMyApprenticeshipCommandHandler(IApprenticeAccountsApiClient apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<string> Handle(CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        return await _apprenticeAccountsApiClient.PostMyApprenticeship(command.ApprenticeId, command, cancellationToken);
    }
}
