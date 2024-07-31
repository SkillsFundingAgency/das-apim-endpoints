using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetClosestRegion
{
    public class GetClosestRegionQueryHandler : IRequestHandler<GetClosestRegionQuery, GetClosestRegionResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _apiClient;

        public GetClosestRegionQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetClosestRegionResult> Handle(GetClosestRegionQuery request, CancellationToken cancellationToken)
        {
            var closestRegion = await _apiClient.GetWithResponseCode<Region>(new GetClosestRegionRequest(request.Latitude, request.Longitude));
            closestRegion.EnsureSuccessStatusCode();

            return new GetClosestRegionResult
            {
                Region = (SharedOuterApi.Models.RequestApprenticeTraining.Region)closestRegion.Body
            };
        }
    }
}