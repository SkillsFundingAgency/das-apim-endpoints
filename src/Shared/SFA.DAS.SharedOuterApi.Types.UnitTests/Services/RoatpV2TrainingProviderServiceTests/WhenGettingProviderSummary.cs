using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;
using System.Net;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.RoatpV2TrainingProviderServiceTests;

public class WhenGettingProviderSummary
{
    [Test, MoqAutoData]
    public async Task Then_If_Response_Is_Successful_Then_Provider_Returned(
        int ukprn,
        GetProviderSummaryResponse apiResponse,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        RoatpV2TrainingProviderService service)
    {
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetProviderSummaryResponse>(
                    It.Is<GetRoatpProviderRequest>(c => c.GetUrl.Contains(ukprn.ToString()))))
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
                x.GetWithResponseCode<GetProviderSummaryResponse>(
                    It.Is<GetRoatpProviderRequest>(c => c.GetUrl.Contains(ukprn.ToString()))))
            .ReturnsAsync(new ApiResponse<GetProviderSummaryResponse>(null, HttpStatusCode.NotFound, "Error"));

        var actual = await service.GetProviderSummary(ukprn);

        actual.Should().BeNull();
    }
}