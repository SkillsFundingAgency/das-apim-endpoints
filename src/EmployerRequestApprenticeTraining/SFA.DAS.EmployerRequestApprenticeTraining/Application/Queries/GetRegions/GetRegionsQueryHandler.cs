using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _apiClient;

        public GetRegionsQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetRegionsResult> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            var regions = await _apiClient.GetWithResponseCode<List<Region>>(new GetRegionsRequest());
            regions.EnsureSuccessStatusCode();

            return new GetRegionsResult
            {
                Regions = regions.Body.Select(region => (SharedOuterApi.Models.RequestApprenticeTraining.Region)region).ToList()
            };
        }
    }
}