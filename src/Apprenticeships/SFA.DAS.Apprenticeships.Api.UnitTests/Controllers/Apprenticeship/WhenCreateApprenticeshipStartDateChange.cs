using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship;

public class WhenCreateApprenticeshipStartDateChange
{
    private readonly Fixture _fixture;

    public WhenCreateApprenticeshipStartDateChange()
    {
        _fixture = new Fixture();
    }

    [Test]
    public async Task ThenCreatesApprenticeshipStartDateChangeUsingApiClient()
    {
        // Arrange
        var apiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        var logger = new Mock<ILogger<ApprenticeshipController>>();
        var sut = new ApprenticeshipController(logger.Object, apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipStartDateChangeRequest>();
        var expectedApiResponse = new ApiResponse<object>("", HttpStatusCode.OK, "");

        apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>(), false))
            .ReturnsAsync(expectedApiResponse);

        // Act
        var response = await sut.CreateApprenticeshipStartDateChange(apprenticeshipKey, request);

        // Assert
        apiClient.Verify(x => x.PostWithResponseCode<object>(It.Is<PostCreateApprenticeshipStartDateChangeRequest>(y =>
            y.ApprenticeshipKey == apprenticeshipKey), false), Times.Once);

        response.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task IfRequestFailsReturnsBadRequest()
    {
        // Arrange
        var apiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        var logger = new Mock<ILogger<ApprenticeshipController>>();
        var sut = new ApprenticeshipController(logger.Object, apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipStartDateChangeRequest>();
        var expectedApiResponse = new ApiResponse<object>("", HttpStatusCode.NotFound, "Has Error");

        apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>(), false))
            .ReturnsAsync(expectedApiResponse);

        // Act
        var response = await sut.CreateApprenticeshipStartDateChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }
}