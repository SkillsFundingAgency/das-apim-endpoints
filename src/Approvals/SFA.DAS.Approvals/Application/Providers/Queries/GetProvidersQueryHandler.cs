using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _apiClient;
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly FeatureToggles _featureToggles;

        public GetProvidersQueryHandler(ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> apiClient,
            IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
            FeatureToggles featureToggles)
        {
            _apiClient = apiClient;
            _providerCoursesApiClient = providerCoursesApiClient;
            _featureToggles = featureToggles;
        }

        public async Task<GetProvidersResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<GetProvidersListItem> providers;

            if (_featureToggles.RoatpProvidersEnabled)
            {
                var result = await _providerCoursesApiClient.Get<GetRoatpProvidersListResponse>(new GetProvidersRequest());

                providers = result.RegisteredProviders;
            }
            else
            {
                var result = await _apiClient.Get<GetProvidersListResponse>(new GetProvidersRequest());

                providers = result.Providers;

            }

            return new GetProvidersResult
            {
                Providers = providers
            };
        }
    }
}