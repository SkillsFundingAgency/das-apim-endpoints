using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;

public class GetApprenticeAccountQueryHandler : IRequestHandler<GetApprenticeAccountQuery, GetApprenticeAccountQueryResult?>
{
    private readonly IApprenticeAccountsApiClient _apprenticeAccountsApiClient;

    public GetApprenticeAccountQueryHandler(IApprenticeAccountsApiClient apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<GetApprenticeAccountQueryResult?> Handle(GetApprenticeAccountQuery request, CancellationToken cancellationToken)
    {
        var apprenticeAccountResponse =
            await _apprenticeAccountsApiClient.GetApprentice(request.ApprenticeId, cancellationToken);

        return apprenticeAccountResponse.ResponseMessage.StatusCode switch
        {
            System.Net.HttpStatusCode.OK => apprenticeAccountResponse.GetContent(),
            System.Net.HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException(
                $"Unexpected response received from apprentice accounts api when getting account details for apprenticeId: {request.ApprenticeId}")
        };
    }
}
