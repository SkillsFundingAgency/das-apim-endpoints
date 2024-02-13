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
        //   var apprenticeAccountResponse = await _apprenticeAccountsApiClient.GetWithResponseCode<GetApprenticeAccountQueryResult?>(new GetApprenticeRequest(request.ApprenticeId));

        var apprenticeAccountResponse =
            await _apprenticeAccountsApiClient.GetApprentice(request.ApprenticeId, cancellationToken);

        if (apprenticeAccountResponse.ResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            return apprenticeAccountResponse.GetContent();
        else if (apprenticeAccountResponse.ResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        else
            throw new InvalidOperationException($"Unexpected response received from apprentice accounts api when getting account details for apprenticeId: {request.ApprenticeId}");
    }
}
