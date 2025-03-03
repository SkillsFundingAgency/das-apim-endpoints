using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpV2ApiClient;

        public GetProviderQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpV2ApiClient)
        {
            _roatpV2ApiClient = roatpV2ApiClient;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var response = await _roatpV2ApiClient.
                GetWithResponseCode<GetProviderSummaryResponse>(new GetRoatpProviderRequest(request.Ukprn));

            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                response.EnsureSuccessStatusCode();
            }

            return new GetProviderResult { Provider = (SharedOuterApi.Models.RoatpV2.Provider)response.Body };
        }
    }
}
