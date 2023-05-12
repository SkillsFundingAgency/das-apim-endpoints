using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

public class CreateMyApprenticeshipCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand, Unit>
{
    private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;

    public CreateMyApprenticeshipCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<Unit> Handle(CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        PostMyApprenticeshipRequest apiRequest = new(command.ApprenticeId) { Data = command };

        var response = await _apprenticeAccountsApiClient.PostWithResponseCode<object>(apiRequest, false);

        if (response.StatusCode == HttpStatusCode.Created) return Unit.Value;

        throw new InvalidOperationException($"An attempt to create MyApprenticeship for ApprenticeId: {command.ApprenticeId}, came back with unsuccessful response status: {response.StatusCode}");
    }
}
