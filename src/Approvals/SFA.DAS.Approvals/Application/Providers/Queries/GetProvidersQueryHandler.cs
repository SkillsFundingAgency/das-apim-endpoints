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

        public GetProvidersQueryHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProvidersResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetProvidersListResponse>(new GetProvidersRequest());
            
            return new GetProvidersResult
            {
                Providers = result.Providers
            };
        }
    }
}