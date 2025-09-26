using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Notifications;
using SFA.DAS.Apprenticeships.Application.Notifications.Handlers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship;

public class WhenApproveDateChange
{
    private readonly Fixture _fixture;
    private readonly Mock<ILearningApiClient<LearningApiConfiguration>> _mockApiClient;
    private readonly ILogger<ApprenticeshipController> _mockedLogger;
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _mockedCommitmentsV2ApiClient;
    private readonly Mock<IMediator> _mockMediator;

    public WhenApproveDateChange()
    {
        _fixture = new Fixture();
        _mockApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _mockedLogger = Mock.Of<ILogger<ApprenticeshipController>>();
        _mockedCommitmentsV2ApiClient = Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _mockMediator = new Mock<IMediator>();
    }

    [Test]
    public async Task ThenApprovesApprenticeshipDateChangeUsingApiClient()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<ApproveStartDateChangeRequest>();
        var innerApiResponse = CreateApiResponse(true);
        var successNotificationResponse = Task.FromResult(new NotificationResponse { Success = true });

        _mockApiClient.Setup(x => x.PatchWithResponseCode<ApproveApprenticeshipStartDateChangeRequest>(It.Is<PatchApproveApprenticeshipStartDateChangeRequest>(i => i.ApprenticeshipKey == apprenticeshipKey)))
            .ReturnsAsync(innerApiResponse)
            .Verifiable();
        _mockMediator.Setup(x => x.Send(It.Is<ChangeOfDateApprovedCommand>(i => i.ApprenticeshipKey == apprenticeshipKey), It.IsAny<CancellationToken>())).Returns(successNotificationResponse)
            .Verifiable();

        // Act
        var response = await sut.ApprovePendingStartDateChange(apprenticeshipKey, request);

        // Assert
        _mockApiClient.VerifyAll();

        response.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task IfApiRequestFailsReturnsBadRequest()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<ApproveStartDateChangeRequest>();
        var innerApiResponse = CreateApiResponse(false);

        _mockApiClient.Setup(x => x.PatchWithResponseCode<ApproveApprenticeshipStartDateChangeRequest>(It.IsAny<PatchApproveApprenticeshipStartDateChangeRequest>()))
           .ReturnsAsync(innerApiResponse)
           .Verifiable();

        // Act
        var response = await sut.ApprovePendingStartDateChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task IfSendNotificationFailsReturnsBadRequest()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<ApproveStartDateChangeRequest>();
        var innerApiResponse = CreateApiResponse(true);
        var failedNotificationResponse = Task.FromResult(new NotificationResponse { Success = false });

        _mockApiClient.Setup(x => x.PatchWithResponseCode<ApproveApprenticeshipStartDateChangeRequest>(It.IsAny<PatchApproveApprenticeshipStartDateChangeRequest>()))
           .ReturnsAsync(innerApiResponse)
           .Verifiable();
        _mockMediator.Setup(x => x.Send(It.IsAny<ChangeOfDateApprovedCommand>(), It.IsAny<CancellationToken>())).Returns(failedNotificationResponse);

        // Act
        var response = await sut.ApprovePendingStartDateChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }

    private ApiResponse<string> CreateApiResponse(bool isSuccessfull)
    {
        var innerApiResponse = _fixture.Create<string>();

        if (isSuccessfull)
        {
            return new ApiResponse<string>(innerApiResponse, HttpStatusCode.OK, "");
        }

        return new ApiResponse<string>(string.Empty, HttpStatusCode.NotFound, "Has Error");
    }
}