using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.RoatpV2TrainingProviderServiceTests;

public class WhenGettingProviderStatus
{
    [Test, MoqAutoData]
    public async Task Then_If_Response_Is_Successful_Then_Provider_Status_Returned(
        int ukprn,
        GetProviderStatusResponse apiResponse,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        RoatpV2TrainingProviderService service)
    {
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetProviderStatusResponse>(
                    It.IsAny<GetRoatpProviderRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderStatusResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await service.GetProvider(ukprn);

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
               x.GetWithResponseCode<GetProviderStatusResponse>(
                   It.IsAny<GetRoatpProviderRequest>()))
           .ReturnsAsync(new ApiResponse<GetProviderStatusResponse>(null, HttpStatusCode.NotFound, "Some Error"));

        var actual = await service.GetProvider(ukprn);
        actual.Should().BeNull();
    }
}