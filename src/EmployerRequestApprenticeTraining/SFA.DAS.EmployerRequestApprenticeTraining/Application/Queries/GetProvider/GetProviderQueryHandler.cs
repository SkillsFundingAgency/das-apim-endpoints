using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Extensions;

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

            return new GetProviderResult { Provider = (SharedOuterApi.Types.Models.RoatpV2.Provider)response.Body };
        }
    }
}
