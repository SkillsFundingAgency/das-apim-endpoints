using System.Net;
using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Regions.Requests;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsQueryResult?>
    {
        private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;

        public GetRegionsQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient) => _apiClient = apiClient;

        public async Task<GetRegionsQueryResult?> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var regionsResult = await _apiClient.GetWithResponseCode<GetRegionsQueryResult>(new GetRegionsQueryRequest());

            if (regionsResult.StatusCode != HttpStatusCode.OK || regionsResult.Body == null) return null;

            return new GetRegionsQueryResult
            {
                Regions = regionsResult.Body.Regions
            };
        }
    }
}