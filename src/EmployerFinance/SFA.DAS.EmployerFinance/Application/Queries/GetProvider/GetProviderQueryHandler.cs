using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpService;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _roatpServiceApiClient;
        private readonly FeatureToggles _featureToggles;

        public GetProviderQueryHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient, IRoatpServiceApiClient<RoatpConfiguration> roatpServiceApiClient, FeatureToggles featureToggles)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _roatpServiceApiClient = roatpServiceApiClient;
            _featureToggles = featureToggles;
        }
        public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            if (_featureToggles.RoatpProvidersEnabled)
            {
                var response = await _roatpServiceApiClient.Get<GetProviderResponse>(
                    new SharedOuterApi.InnerApi.Requests.RoatpService.GetProviderRequest(request.Id));

                return new GetProviderQueryResult
                {
                    Ukprn = response.Ukprn,
                    Name = response.LegalName,
                    ContactUrl = response.Website,
                    Email = response.Email,
                    Phone = response.Phone
                };
            }
            else
            {
                var response = await _courseDeliveryApiClient.Get<GetProvidersListItem>(new GetProviderRequest(request.Id));

                return new GetProviderQueryResult
                {
                    Ukprn = response.Ukprn,
                    Name = response.Name,
                    ContactUrl = response.ContactUrl,
                    Email = response.Email,
                    Phone = response.Phone
                };
            }
        }
    }
}