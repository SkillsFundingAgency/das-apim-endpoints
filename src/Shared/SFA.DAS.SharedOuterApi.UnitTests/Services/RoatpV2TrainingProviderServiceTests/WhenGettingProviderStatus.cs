using System.Net;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.RoatpV2TrainingProviderServiceTests;

public class WhenGettingProviderStatus
{
    [Test, MoqAutoData]
    public async Task Then_If_Response_Is_Successful_Then_Provider_Status_Returned(
        int ukprn,
        GetProviderSummaryResponse apiResponse,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        RoatpV2TrainingProviderService service)
    {
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetProviderSummaryResponse>(
                    It.IsAny<GetRoatpProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderSummaryResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await service.GetProviderSummary(ukprn);

        actual.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Response_Is_Not_Successful_Then_Null(
        int ukprn,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        RoatpV2TrainingProviderService service)
    {
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetProvidersResponse>(
                    It.IsAny<GetRoatpProvidersRequest>()))
            .ReturnsAsync(new ApiResponse<GetProvidersResponse>(null, HttpStatusCode.NotFound, "Error"));

        apiClient.Setup(x =>
               x.GetWithResponseCode<GetProviderSummaryResponse>(
                   It.IsAny<GetRoatpProviderRequest>()))
           .ReturnsAsync(new ApiResponse<GetProviderSummaryResponse>(null, HttpStatusCode.NotFound, "Some Error"));

        var actual = await service.GetProviderSummary(ukprn);
        actual.Should().BeNull();
    }
}