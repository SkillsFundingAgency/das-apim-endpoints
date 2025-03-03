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

public class WhenGettingProviders
{
    [Test, MoqAutoData]
    public async Task Then_If_Response_Is_Successful_Then_Providers_Returned(
        int ukprn,
        GetProvidersResponse apiResponse,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        RoatpV2TrainingProviderService service)
    {
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetProvidersResponse>(
                    It.IsAny<GetRoatpProvidersRequest>()))
            .ReturnsAsync(new ApiResponse<GetProvidersResponse>(apiResponse, HttpStatusCode.OK, ""));

        var actual = await service.GetProviders(new CancellationToken());

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

        var actual = await service.GetProviders(new CancellationToken());
        actual.Should().BeNull();
    }
}