using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;

public class GetStagedApprenticeQueryHandler : IRequestHandler<GetStagedApprenticeQuery, GetStagedApprenticeQueryResult?>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetStagedApprenticeQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetStagedApprenticeQueryResult?> Handle(GetStagedApprenticeQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.GetStagedApprentice(request, cancellationToken);

        return (result.ResponseMessage.StatusCode) switch
        {
            HttpStatusCode.OK => result.GetContent(),
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException($"GetStagedApprentice inner api call did not come back with success statusCode: {result.ResponseMessage.StatusCode}")
        };
    }
}