using RestEase;
using SFA.DAS.AdminAan.Application.Regions.Queries.GetRegions;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IAanHubRestApiClient
{
    [Get("/regions")]
    Task<GetRegionsQueryResult> GetRegions(CancellationToken cancellationToken);
}
