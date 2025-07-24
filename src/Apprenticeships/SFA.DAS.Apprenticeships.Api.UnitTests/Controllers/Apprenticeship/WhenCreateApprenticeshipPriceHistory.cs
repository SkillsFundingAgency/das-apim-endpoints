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
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship;

public class WhenCreateApprenticeshipPriceHistory
{
    private readonly Fixture _fixture;
    private readonly Mock<ILearningApiClient<LearningApiConfiguration>> _mockApiClient;
    private readonly ILogger<ApprenticeshipController> _mockedLogger;
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _mockedCommitmentsV2ApiClient;
    private readonly Mock<IMediator> _mockMediator;

    public WhenCreateApprenticeshipPriceHistory()
    {
		_fixture = new Fixture();
        _mockApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _mockedLogger = Mock.Of<ILogger<ApprenticeshipController>>();
        _mockedCommitmentsV2ApiClient = Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _mockMediator = new Mock<IMediator>();
    }

    [Test]
    public async Task ThenCreatesApprenticeshipPriceHistoryUsingApiClient()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);
            
        // Arrange
        var apprenticeshipKey = Guid.NewGuid();
        var request = new PostCreateApprenticeshipPriceChangeRequest(
            apprenticeshipKey: apprenticeshipKey,
            initiator: "Provider",
            userId: "testUser",
            trainingPrice: 1000,
            assessmentPrice: 500,
            totalPrice: 1500,
            reason: "Test Reason",
            effectiveFromDate: new DateTime(2023, 04, 04));

        _mockApiClient.Setup(x => x.PostWithResponseCode<PostCreateApprenticeshipPriceChangeApiResponse>(It.IsAny<PostCreateApprenticeshipPriceChangeRequest>(), It.IsAny<bool>())).ReturnsAsync(new ApiResponse<PostCreateApprenticeshipPriceChangeApiResponse>(new PostCreateApprenticeshipPriceChangeApiResponse(), HttpStatusCode.OK, ""));
        _mockMediator.Setup(x => x.Send(It.IsAny<ChangeOfPriceInitiatedCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new NotificationResponse { Success = true }));

        // Act
        var response = await sut.CreateApprenticeshipPriceChange(
            request.ApprenticeshipKey,
            new Models.CreateApprenticeshipPriceChangeRequest
            {
                Initiator = ((CreateApprenticeshipPriceChangeRequest)request.Data).Initiator,
                UserId = ((CreateApprenticeshipPriceChangeRequest)request.Data).UserId,
                TrainingPrice = ((CreateApprenticeshipPriceChangeRequest)request.Data).TrainingPrice,
                AssessmentPrice = ((CreateApprenticeshipPriceChangeRequest)request.Data).AssessmentPrice,
                TotalPrice = ((CreateApprenticeshipPriceChangeRequest)request.Data).TotalPrice,
                Reason = ((CreateApprenticeshipPriceChangeRequest)request.Data).Reason,
                EffectiveFromDate = ((CreateApprenticeshipPriceChangeRequest)request.Data).EffectiveFromDate
            });

        // Assert
        _mockApiClient.Verify(x => x.PostWithResponseCode<object>(It.Is<PostCreateApprenticeshipPriceChangeRequest>(r =>
            ((CreateApprenticeshipPriceChangeRequest)r.Data).Initiator == ((CreateApprenticeshipPriceChangeRequest)request.Data).Initiator &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).UserId == ((CreateApprenticeshipPriceChangeRequest)request.Data).UserId &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).TrainingPrice == ((CreateApprenticeshipPriceChangeRequest)request.Data).TrainingPrice &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).AssessmentPrice == ((CreateApprenticeshipPriceChangeRequest)request.Data).AssessmentPrice &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).TotalPrice == ((CreateApprenticeshipPriceChangeRequest)request.Data).TotalPrice &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).Reason == ((CreateApprenticeshipPriceChangeRequest)request.Data).Reason &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).EffectiveFromDate == ((CreateApprenticeshipPriceChangeRequest)request.Data).EffectiveFromDate &&
            r.ApprenticeshipKey == request.ApprenticeshipKey), It.IsAny<bool>()), Times.Once);
        response.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public async Task IfApiRequestFailsReturnsBadRequest()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipPriceChangeRequest>();
        _mockApiClient.Setup(x => x.PostWithResponseCode<PostCreateApprenticeshipPriceChangeApiResponse>(It.IsAny<PostCreateApprenticeshipPriceChangeRequest>(), It.IsAny<bool>())).ReturnsAsync(new ApiResponse<PostCreateApprenticeshipPriceChangeApiResponse>(null, HttpStatusCode.NotFound, "Has Error"));

        // Act
        var response = await sut.CreateApprenticeshipPriceChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task IfSendNotificationFailsReturnsBadRequest()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<Models.CreateApprenticeshipPriceChangeRequest>();
        _mockApiClient.Setup(x => x.PostWithResponseCode<PostCreateApprenticeshipPriceChangeApiResponse>(It.IsAny<PostCreateApprenticeshipPriceChangeRequest>(), It.IsAny<bool>())).ReturnsAsync(new ApiResponse<PostCreateApprenticeshipPriceChangeApiResponse>(new PostCreateApprenticeshipPriceChangeApiResponse(), HttpStatusCode.OK, ""));
        _mockMediator.Setup(x => x.Send(It.IsAny<ChangeOfPriceInitiatedCommand>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new NotificationResponse { Success = false }));
        
        // Act
        var response = await sut.CreateApprenticeshipPriceChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }
}