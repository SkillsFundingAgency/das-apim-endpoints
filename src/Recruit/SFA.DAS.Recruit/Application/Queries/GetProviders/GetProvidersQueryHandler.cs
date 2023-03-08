using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetProviders
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersQueryResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        public GetProvidersQueryHandler (IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
        {
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
        }
        public async Task<GetProvidersQueryResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _roatpCourseManagementApiClient.Get<GetProvidersListResponse>(new GetProvidersRequest());
            
            return new GetProvidersQueryResult
            {
                Providers = response.RegisteredProviders
            };
        }
    }
}