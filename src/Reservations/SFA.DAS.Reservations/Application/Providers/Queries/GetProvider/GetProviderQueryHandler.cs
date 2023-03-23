using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Providers.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _roatpApiClient;
        private readonly FeatureToggles _featureToggles;

        public GetProviderQueryHandler(ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            IRoatpServiceApiClient<RoatpConfiguration> roatpApiClient,
            FeatureToggles featureToggles)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _roatpApiClient = roatpApiClient;
            _featureToggles = featureToggles;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            GetProviderResponse provider=null;

            if (_featureToggles.RoatpProvidersEnabled)
            {
                var result = await _roatpApiClient.Get<GetRoatpProviderResponse>(
                    new GetProviderRequest
                    {
                        Ukprn = request.Ukprn
                    });

                provider = result;
            }
            else
            {
                provider = await _courseDeliveryApiClient.Get<GetProviderResponse>(
                    new GetProviderRequest
                    {
                        Ukprn = request.Ukprn
                    });
            }

            return new GetProviderResult
            {
                Provider = provider
            };
        }
    }
}