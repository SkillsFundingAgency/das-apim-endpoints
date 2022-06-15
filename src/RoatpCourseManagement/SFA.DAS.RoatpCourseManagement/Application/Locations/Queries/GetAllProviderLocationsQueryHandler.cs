using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries
{
    public class GetAllProviderLocationsQueryHandler : IRequestHandler<GetAllProviderLocationsQuery, GetAllProviderLocationsQueryResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;

        public GetAllProviderLocationsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
        {
            _courseManagementApiClient = courseManagementApiClient;
        }

        public async Task<GetAllProviderLocationsQueryResult> Handle(GetAllProviderLocationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseManagementApiClient.Get<List<ProviderLocationModel>>(request);
            return new GetAllProviderLocationsQueryResult() { ProviderLocations = response };
        }
    }
}
