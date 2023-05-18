using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;

public class GetStagedApprenticeQueryHandler : IRequestHandler<GetStagedApprenticeQuery, GetStagedApprenticeQueryResult?>
{
    private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;

    public GetStagedApprenticeQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetStagedApprenticeQueryResult?> Handle(GetStagedApprenticeQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetWithResponseCode<GetStagedApprenticeQueryResult?>(new GetStagedApprenticeRequest(request.LastName, request.DateOfBirth, request.Email));

        return (result.StatusCode) switch
        {
            HttpStatusCode.OK => result.Body,
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException($"GetStagedApprentice inner api call did not come back with success statusCode: {result.StatusCode}")
        };
    }
}