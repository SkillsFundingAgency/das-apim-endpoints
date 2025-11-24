using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.RegisteredProviders.Queries;

public class GetRegisteredProvidersQueryHandler : IRequestHandler<GetRegisteredProvidersQuery, ApiResponse<RegisteredProviderResponse>>
{
    private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;
    private readonly ILogger<GetRegisteredProvidersQueryHandler> _logger;
    public GetRegisteredProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient, ILogger<GetRegisteredProvidersQueryHandler> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<ApiResponse<RegisteredProviderResponse>> Handle(GetRegisteredProvidersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get all registered providers");
        var apiResponse = await _apiClient.GetWithResponseCode<RegisteredProviderResponse>(new GetOrganisationsRequest());

        return apiResponse;
    }
}
