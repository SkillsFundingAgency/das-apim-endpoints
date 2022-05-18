using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries
{

    public class GetRegisteredProvidersQueryHandler : IRequestHandler<GetRegisteredProvidersQuery, ApiResponse<List<RegisteredProviderModel>>>
    {
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;

        public GetRegisteredProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }


        public async Task<ApiResponse<List<RegisteredProviderModel>>> Handle(GetRegisteredProvidersQuery request, CancellationToken cancellationToken)

        {
            return await _apiClient.GetWithResponseCode<List<RegisteredProviderModel>>(request);
        }
    }
}
