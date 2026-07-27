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
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.UkrlpData.Queries.GetUkrlpProviders;

public class GetUkrlpProvidersQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenGettingProvidersFromUkrlp_InvokesUkrlpWithCorrectParams(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        GetUkrlpProvidersQueryHandler sut,
        GetUkrlpProvidersQuery query)
    {
        // Arrange
        roatpServiceApiClientMock
            .Setup(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.IsAny<GetUkrlpProvidersRequest>()))
            .ReturnsAsync(new ApiResponse<UkrlpProvidersResponse>(new UkrlpProvidersResponse([]), System.Net.HttpStatusCode.OK, null));
        // Act
        await sut.Handle(query, default);
        // Assert
        roatpServiceApiClientMock
            .Verify(x => x.GetWithResponseCode<UkrlpProvidersResponse>(It.Is<GetUkrlpProvidersRequest>(r => r.UpdatedSinceDate == query.UpdatedSinceDate && r.Ukprns.SequenceEqual(query.Ukprns))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGettingProvidersFromUkrlp_InvokesUkrlpEndPointInBatches(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        GetUkrlpProvidersQueryHandler sut,
        Generator<int> ukprns,
        GetUkrlpProvidersQuery query)
    {
        query.Ukprns = ukprns.Take(150);
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
