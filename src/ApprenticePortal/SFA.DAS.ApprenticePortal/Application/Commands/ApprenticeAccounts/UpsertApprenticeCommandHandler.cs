using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticePortal.Application.Commands.ApprenticeAccounts;

public class UpsertApprenticeCommandHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient) : IRequestHandler<UpsertApprenticeCommand, UpsertApprenticeCommandResult>
{
    public async Task<UpsertApprenticeCommandResult> Handle(UpsertApprenticeCommand request, CancellationToken cancellationToken)
    {
        var result = await accountsApiClient.PutWithResponseCode<Apprentice>(new PutApprenticeApiRequest(
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