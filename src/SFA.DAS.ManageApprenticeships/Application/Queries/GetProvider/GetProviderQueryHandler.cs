using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public GetProviderQueryHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseDeliveryApiClient.Get<GetProvidersListItem>(new GetProviderRequest(request.Id));
            
            return new GetProviderQueryResult
            {
                Provider = response
            };
        }
    }
}