using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class RoatpV2TrainingProviderService : IRoatpV2TrainingProviderService
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        public RoatpV2TrainingProviderService(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
        {
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
        }

        public async Task<GetProviderSummaryResponse> GetProviderSummary(int ukprn)
        {
            var actual =
                await _roatpCourseManagementApiClient.GetWithResponseCode<GetProviderSummaryResponse>(
                    new GetRoatpProviderRequest(ukprn));

            return ApiResponseErrorChecking.IsSuccessStatusCode(actual.StatusCode) ? actual.Body : null;
        }
    }

    
}