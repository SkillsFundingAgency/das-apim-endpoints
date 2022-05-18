using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.ProviderRegistration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries
{

    public class GetRegisteredProvidersQueryHandler : IRequestHandler<GetRegisteredProvidersQuery, GetRegisteredProvidersQueryResult>
    {
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _apiClient;

        public GetRegisteredProvidersQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }


        public async Task<GetRegisteredProvidersQueryResult> Handle(GetRegisteredProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<List<RegisteredProviderModel>>(request);
            return new GetRegisteredProvidersQueryResult { Providers = response.Body };
        }
    }
}
