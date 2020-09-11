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

        public GetProviderQueryHandler(ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var provider = await _courseDeliveryApiClient.Get<GetProviderResponse>(
                new GetProviderRequest
                {
                    Ukprn = request.Ukprn
                });

            return new GetProviderResult
            {
                Provider = provider
            };
        }
    }
}