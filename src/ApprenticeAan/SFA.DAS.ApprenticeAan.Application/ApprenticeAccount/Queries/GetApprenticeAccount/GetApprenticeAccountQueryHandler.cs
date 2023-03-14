using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;

public class GetApprenticeAccountQueryHandler : IRequestHandler<GetApprenticeAccountQuery, GetApprenticeAccountQueryResult?>
{
    private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;

    public GetApprenticeAccountQueryHandler(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient)
    {
        _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
    }

    public async Task<GetApprenticeAccountQueryResult?> Handle(GetApprenticeAccountQuery request, CancellationToken cancellationToken)
    {
        var apprenticeAccountResponse = await _apprenticeAccountsApiClient.GetWithResponseCode<GetApprenticeAccountQueryResult?>(new GetApprenticeRequest(request.ApprenticeId));

        if (apprenticeAccountResponse.StatusCode == System.Net.HttpStatusCode.OK)
            return apprenticeAccountResponse.Body;
        else if (apprenticeAccountResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        else
            throw new InvalidOperationException($"Unexpected response received from apprentice accounts api when getting account details for apprenticeId: {request.ApprenticeId}");
    }
}
