using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AdminRoatp.InnerApi.Requests.Roatp;
using SFA.DAS.AdminRoatp.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedList;
public class GetProvidersAllowedListQueryHandler(IApplyApiClient<ApplyApiConfiguration> _apiClient, ILogger<GetProvidersAllowedListQueryHandler> _logger) : IRequestHandler<GetProvidersAllowedListQuery, GetProvidersAllowedListQueryResponse>
{
    public async Task<GetProvidersAllowedListQueryResponse> Handle(GetProvidersAllowedListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get providers allowed list request received with sort column {SortColumn} and sort order {SortOrder}", request.sortColumn, request.sortOrder);

        var response = await _apiClient.GetWithResponseCode<List<AllowedProvider>>(new GetAllowedProvidersListRequest(request.sortColumn, request.sortOrder));

        response.EnsureSuccessStatusCode();

        return new() { Providers = response.Body };
    }
}