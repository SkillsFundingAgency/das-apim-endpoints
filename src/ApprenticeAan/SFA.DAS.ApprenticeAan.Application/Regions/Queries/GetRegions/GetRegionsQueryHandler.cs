using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;

public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsQueryResult>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetRegionsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetRegionsQueryResult> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
    {
        return await _apiClient.GetRegions();
    }
}