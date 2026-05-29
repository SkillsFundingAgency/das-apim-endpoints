using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class RoatpV2TrainingProviderService(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
    : IRoatpV2TrainingProviderService
{
    public async Task<GetProviderSummaryResponse> GetProviderSummary(int ukprn)
    {
        var actual =
            await roatpCourseManagementApiClient.GetWithResponseCode<GetProviderSummaryResponse>(
                new GetRoatpProviderRequest(ukprn));

        return ApiResponseErrorChecking.IsSuccessStatusCode(actual.StatusCode) ? actual.Body : null;
    }

    public async Task<GetProvidersResponse> GetProviders(CancellationToken cancellationToken)
    {
        var actual =
            await roatpCourseManagementApiClient.GetWithResponseCode<GetProvidersResponse>(
                new GetRoatpProvidersRequest());

        return ApiResponseErrorChecking.IsSuccessStatusCode(actual.StatusCode) ? actual.Body : null;
    }

    public async Task<GetProvidersResponse> GetProviders(bool live)
    {
        var actual =
            await roatpCourseManagementApiClient.GetWithResponseCode<GetProvidersResponse>(
                new GetRoatpProvidersRequest { Live = live });

        return ApiResponseErrorChecking.IsSuccessStatusCode(actual.StatusCode) ? actual.Body : null;
    }
}