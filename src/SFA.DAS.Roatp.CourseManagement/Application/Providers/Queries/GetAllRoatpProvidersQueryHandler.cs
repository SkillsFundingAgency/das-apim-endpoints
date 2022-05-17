using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Providers.Queries
{

    public class GetAllRoatpProvidersQueryHandler : IRequestHandler<GetAllRoatpProvidersQuery, GetAllRoatpProvidersQueryResult>
    {
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;

        public GetAllRoatpProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }


        public async Task<GetAllRoatpProvidersQueryResult> Handle(GetAllRoatpProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<List<RoatpProviderModel>>(request);
            return new GetAllRoatpProvidersQueryResult { Providers = response.Body };
        }
    }
}
