using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Regions.Requests;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsQueryResult?>
    {
        private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;
        private readonly ILogger<GetRegionsQueryHandler> _logger;

        public GetRegionsQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient, ILogger<GetRegionsQueryHandler> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<GetRegionsQueryResult?> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionsResult = await _apiClient.GetWithResponseCode<GetRegionsQueryResult>(new GetRegionsQueryRequest());

            if (regionsResult.StatusCode == HttpStatusCode.OK && regionsResult.Body != null)
                return new GetRegionsQueryResult
                {
                    Regions = regionsResult.Body.Regions
                };

            _logger.LogError("ApprenticeAan Outer API: Unable to query AanHub API /Regions endpoint");
            return null;
        }
    }
}