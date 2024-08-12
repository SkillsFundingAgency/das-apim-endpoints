using MediatR;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Application.Commands;

public class UpsertApprenticeCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient) : IRequestHandler<UpsertApprenticeCommand, UpsertApprenticeCommandResult>
{
    public async Task<UpsertApprenticeCommandResult> Handle(UpsertApprenticeCommand request, CancellationToken cancellationToken)
    {
        var result = await accountsApiClient.PutWithResponseCode<Models.Apprentice>(new PutApprenticeApiRequest(
            new PutApprenticeApiRequestData
            {
                Email = request.Email,
                GovUkIdentifier = request.GovUkIdentifier
            }));

        return new UpsertApprenticeCommandResult
        {
            Apprentice = result.Body
        };
    }
}