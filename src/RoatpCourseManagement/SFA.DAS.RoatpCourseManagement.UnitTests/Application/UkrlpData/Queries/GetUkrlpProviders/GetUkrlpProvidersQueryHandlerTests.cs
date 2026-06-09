using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData.Queries.GetUkrlpProviders;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.UkrlpData.Queries.GetUkrlpProviders;

public class GetUkrlpProvidersQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenGettingProvidersFromUkrlp_GetsAllMainProvidersFromRoatpCourseManagementApi(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        GetUkrlpProvidersQueryHandler sut,
        GetUkrlpProvidersQuery query,
        GetProvidersResponse response)
    {
        // Arrange
        courseManagementApiClientMock
            .Setup(x => x.GetWithResponseCode<GetProvidersResponse>(It.Is<GetRoatpProvidersRequest>(r => r.Live.GetValueOrDefault())))
            .ReturnsAsync(new ApiResponse<GetProvidersResponse>(response, System.Net.HttpStatusCode.OK, null));
        roatpServiceApiClientMock
            .Setup(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.IsAny<GetUkrlpProvidersRequest>()))
            .ReturnsAsync(new ApiResponse<UkrlpProvidersResponse>(new UkrlpProvidersResponse([]), System.Net.HttpStatusCode.OK, null));
        // Act
        await sut.Handle(query, default);
        // Assert
        courseManagementApiClientMock.Verify(x => x.GetWithResponseCode<GetProvidersResponse>(It.Is<GetRoatpProvidersRequest>(r => r.Live.GetValueOrDefault())), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersFromUkrlp_InvokesUkrlpWithCorrectParams(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        GetUkrlpProvidersQueryHandler sut,
        DateTime expectedUpdatedSinceDate,
        GetProvidersResponse response)
    {
        // Arrange
        GetUkrlpProvidersQuery query = new(expectedUpdatedSinceDate);

        courseManagementApiClientMock
            .Setup(x => x.GetWithResponseCode<GetProvidersResponse>(It.Is<GetRoatpProvidersRequest>(r => r.Live.GetValueOrDefault())))
            .ReturnsAsync(new ApiResponse<GetProvidersResponse>(response, System.Net.HttpStatusCode.OK, null));
        roatpServiceApiClientMock
            .Setup(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.IsAny<GetUkrlpProvidersRequest>()))
            .ReturnsAsync(new ApiResponse<UkrlpProvidersResponse>(new UkrlpProvidersResponse([]), System.Net.HttpStatusCode.OK, null));
        // Act
        await sut.Handle(query, default);
        // Assert
        roatpServiceApiClientMock
            .Verify(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.Is<GetUkrlpProvidersRequest>(r => r.UpdatedSinceDate == expectedUpdatedSinceDate && r.Ukprns.SequenceEqual(response.RegisteredProviders.Select(p => p.Ukprn)))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersFromUkrlp_InvokesUkrlpEndPointInBatches(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        GetUkrlpProvidersQueryHandler sut,
        Generator<Provider> providers,
        GetUkrlpProvidersQuery query,
        DateTime expectedUpdatedSinceDate)
    {
        GetProvidersResponse response = new() { RegisteredProviders = providers.Take(150) };
        courseManagementApiClientMock
            .Setup(x => x.GetWithResponseCode<GetProvidersResponse>(It.Is<GetRoatpProvidersRequest>(r => r.Live.GetValueOrDefault())))
            .ReturnsAsync(new ApiResponse<GetProvidersResponse>(response, System.Net.HttpStatusCode.OK, null));
        roatpServiceApiClientMock
            .Setup(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.IsAny<GetUkrlpProvidersRequest>()))
            .ReturnsAsync(new ApiResponse<UkrlpProvidersResponse>(new UkrlpProvidersResponse([]), System.Net.HttpStatusCode.OK, null));
        // Act
        await sut.Handle(query, default);
        // Assert
        roatpServiceApiClientMock.Verify(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.Is<GetUkrlpProvidersRequest>(r => r.Ukprns.Count() == 100)), Times.Once);
        roatpServiceApiClientMock.Verify(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.Is<GetUkrlpProvidersRequest>(r => r.Ukprns.Count() == 50)), Times.Once);
    }
}
