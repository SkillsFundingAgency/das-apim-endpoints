using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetRemovedReasons;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetRemovedReasons;
public class GetRemovedReasonsQueryHandlerTests
{
    [Test, MoqAutoData]

    public async Task Handle_RemovedReasonsFound_ReturnsRemovedReasons(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetRemovedReasonsQueryHandler sut,
        GetRemovedReasonsQuery query,
        GetRemovedReasonsResponse expectedResponse)
    {
        // Arrange
        apiClient.Setup(a => a.GetWithResponseCode<GetRemovedReasonsResponse>(It.Is<GetRemovedReasonsRequest>(r => r.GetUrl.Equals(new GetRemovedReasonsRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetRemovedReasonsResponse>(expectedResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        apiClient.Verify(a => a.GetWithResponseCode<GetRemovedReasonsResponse>(It.Is<GetRemovedReasonsRequest>(r => r.GetUrl.Equals(new GetRemovedReasonsRequest().GetUrl))), Times.Once);
    }

    [Test, MoqAutoData]

    public async Task Handle_RemovedReasonsNotFound_ReturnsEmpty(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetRemovedReasonsQueryHandler sut,
        GetRemovedReasonsQuery query)
    {
        // Arrange
        GetRemovedReasonsResponse expectedResponse = new();
        apiClient.Setup(a => a.GetWithResponseCode<GetRemovedReasonsResponse>(It.Is<GetRemovedReasonsRequest>(r => r.GetUrl.Equals(new GetRemovedReasonsRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetRemovedReasonsResponse>(expectedResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.ReasonsForRemoval.Should().BeEmpty();
    }
}