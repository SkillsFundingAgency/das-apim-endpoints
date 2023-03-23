using MediatR;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Regions.Requests;
using SFA.DAS.ApprenticeAan.Application.Services;

namespace SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsQueryResult?>
    {
        private readonly IAanHubApiClient<AanHubApiConfiguration> _apiClient;

        public GetRegionsQueryHandler(IAanHubApiClient<AanHubApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetRegionsQueryResult?> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            return await _apiClient.Get<GetRegionsQueryResult>(new GetRegionsQueryRequest());
        }
    }
}