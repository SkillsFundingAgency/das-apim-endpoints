using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;

public class CreateMyApprenticeshipCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand, Unit>
{
    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;

    public CreateMyApprenticeshipCommandHandler(IApprenticeAccountsApiClient apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<Unit> Handle(CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        var response = await _apprenticeAccountsApiClient.PostMyApprenticeship(command.ApprenticeId, command, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Created) return Unit.Value;
        throw new InvalidOperationException($"An attempt to create MyApprenticeship for ApprenticeId: {command.ApprenticeId}, came back with unsuccessful response status: {response.StatusCode}");
    }
}