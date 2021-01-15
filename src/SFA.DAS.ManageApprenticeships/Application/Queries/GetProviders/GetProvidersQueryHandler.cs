using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetProviders
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersQueryResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public GetProvidersQueryHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<GetProvidersQueryResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersRequest());
            
            return new GetProvidersQueryResult
            {
                Providers = response.Providers
            };
        }
    }
}