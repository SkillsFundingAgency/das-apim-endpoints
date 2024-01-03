using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Application.LeavingReasons.Queries.GetLeavingReasons;

public class GetLeavingReasonsQueryHandler : IRequestHandler<GetLeavingReasonsQuery, List<LeavingCategory>>
{
    private readonly IAanHubRestApiClient _apiClient;

    public GetLeavingReasonsQueryHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<LeavingCategory>> Handle(GetLeavingReasonsQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetLeavingReasons(cancellationToken);
        return response.LeavingCategories;
    }
}