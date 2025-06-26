using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
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
        var apiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        var logger = new Mock<ILogger<ApprenticeshipController>>();
        var mediator = new Mock<IMediator>();
        var sut = new ApprenticeshipController(logger.Object, apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), mediator.Object);

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipStartDateChangeRequest>();
        var expectedApiResponse = new ApiResponse<object>("", HttpStatusCode.OK, "");
        mediator.Setup(x => x.Send(It.IsAny<ChangeOfStartDateInitiatedCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new NotificationResponse { Success = true }));

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
        var apiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        var logger = new Mock<ILogger<ApprenticeshipController>>();
        var mediator = new Mock<IMediator>();
        var sut = new ApprenticeshipController(logger.Object, apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), mediator.Object);

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipStartDateChangeRequest>();
        var expectedApiResponse = new ApiResponse<object>("", HttpStatusCode.NotFound, "Has Error");

        apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>(), false))
            .ReturnsAsync(expectedApiResponse);
        mediator.Setup(x => x.Send(It.IsAny<ChangeOfStartDateInitiatedCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new NotificationResponse { Success = true }));

        // Act
        var response = await sut.CreateApprenticeshipStartDateChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task IfSendNotificationFailsReturnsBadRequest()
    {
        // Arrange
        var apiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        var logger = new Mock<ILogger<ApprenticeshipController>>();
        var mediator = new Mock<IMediator>();
        var sut = new ApprenticeshipController(logger.Object, apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), mediator.Object);

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipStartDateChangeRequest>();
        var expectedApiResponse = new ApiResponse<object>("", HttpStatusCode.OK, "");
        mediator.Setup(x => x.Send(It.IsAny<ChangeOfStartDateInitiatedCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new NotificationResponse { Success = false }));

        apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>(), false))
            .ReturnsAsync(expectedApiResponse);

        // Act
        var response = await sut.CreateApprenticeshipStartDateChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }
}