using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetProviderLocationDetails
{
    public class GetProviderLocationDetailsQueryHandler : IRequestHandler<GetProviderLocationDetailsQuery, GetProviderLocationDetailsQueryResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;

        public GetProviderLocationDetailsQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient)
        {
            _courseManagementApiClient = courseManagementApiClient;
        }

        public async Task<GetProviderLocationDetailsQueryResult> Handle(GetProviderLocationDetailsQuery request, CancellationToken cancellationToken)
        {
            var response = await _courseManagementApiClient.GetWithResponseCode<ProviderLocationModel>(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return new GetProviderLocationDetailsQueryResult() { ProviderLocation = response.Body };
        }
    }
}
