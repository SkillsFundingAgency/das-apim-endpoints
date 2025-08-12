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
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InnerApiRequest = SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning.PatchApproveApprenticeshipPriceChangeRequest;
using InnerApiResponse = SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning.PatchApproveApprenticeshipPriceChangeResponse;
using OuterApiRequest = SFA.DAS.Apprenticeships.Api.Models.ApprovePriceChangeRequest;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship;

public class WhenApprovePriceChange
{
    private readonly Fixture _fixture;
    private readonly Mock<ILearningApiClient<LearningApiConfiguration>> _mockApiClient;
    private readonly ILogger<ApprenticeshipController> _mockedLogger;
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _mockedCommitmentsV2ApiClient;
    private readonly Mock<IMediator> _mockMediator;

    public WhenApprovePriceChange()
    {
        _fixture = new Fixture();
        _mockApiClient = new Mock<ILearningApiClient<LearningApiConfiguration>>();
        _mockedLogger = Mock.Of<ILogger<ApprenticeshipController>>();
        _mockedCommitmentsV2ApiClient = Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
        _mockMediator = new Mock<IMediator>();
    }

    [Test]
    public async Task ThenApprovesApprenticeshipPriceHistoryUsingApiClient()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<OuterApiRequest>();
        var innerApiResponse = CreateApiResponse(true);
        var successNotificationResponse = Task.FromResult(new NotificationResponse { Success = true });

        _mockApiClient.Setup(x => x.PatchWithResponseCode<ApproveApprenticeshipPriceChangeRequest, InnerApiResponse>(It.IsAny<InnerApiRequest>(), It.IsAny<bool>())).ReturnsAsync(innerApiResponse);
        _mockMediator.Setup(x => x.Send(It.IsAny<ChangeOfPriceApprovedCommand>(), It.IsAny<CancellationToken>())).Returns(successNotificationResponse);

        // Act
        var response = await sut.ApprovePendingPriceChange(apprenticeshipKey, request);

        // Assert
        _mockApiClient.Verify(x => x.PatchWithResponseCode<ApproveApprenticeshipPriceChangeRequest, InnerApiResponse>(
            It.Is<InnerApiRequest>(r => IsValidApiRequest(apprenticeshipKey, r, request)), It.IsAny<bool>()), Times.Once);

        response.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task IfApiRequestFailsReturnsBadRequest()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<OuterApiRequest>();
        var innerApiResponse = CreateApiResponse(false);

        _mockApiClient.Setup(x => x.PatchWithResponseCode<ApproveApprenticeshipPriceChangeRequest, InnerApiResponse>(It.IsAny<InnerApiRequest>(), It.IsAny<bool>())).ReturnsAsync(innerApiResponse);

        // Act
        var response = await sut.ApprovePendingPriceChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task IfSendNotificationFailsReturnsBadRequest()
    {
        var sut = new ApprenticeshipController(_mockedLogger, _mockApiClient.Object, _mockedCommitmentsV2ApiClient, _mockMediator.Object);

        // Arrange
        var apprenticeshipKey = _fixture.Create<Guid>();
        var request = _fixture.Create<OuterApiRequest>();
        var innerApiResponse = CreateApiResponse(true);
        var failedNotificationResponse = Task.FromResult(new NotificationResponse { Success = false });

        _mockApiClient.Setup(x => x.PatchWithResponseCode<ApproveApprenticeshipPriceChangeRequest, InnerApiResponse>(It.IsAny<InnerApiRequest>(), It.IsAny<bool>())).ReturnsAsync(innerApiResponse);
        _mockMediator.Setup(x => x.Send(It.IsAny<ChangeOfPriceApprovedCommand>(), It.IsAny<CancellationToken>())).Returns(failedNotificationResponse);

        // Act
        var response = await sut.ApprovePendingPriceChange(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }

    private ApiResponse<InnerApiResponse> CreateApiResponse(bool isSuccessfull)
    {
        var innerApiResponse = _fixture.Create<InnerApiResponse>();

        if (isSuccessfull)
        {
            return new ApiResponse<InnerApiResponse>(innerApiResponse, HttpStatusCode.OK, "");
        }

        return new ApiResponse<InnerApiResponse>(null, HttpStatusCode.NotFound, "Has Error");
    }

    private static bool IsValidApiRequest(
        Guid apprenticeshipKey,
        InnerApiRequest innerApiRequest,
        OuterApiRequest outerApiRequest)
    {
        return (innerApiRequest.ApprenticeshipKey == apprenticeshipKey) &&
            (innerApiRequest.Data.UserId == outerApiRequest.UserId) &&
            (innerApiRequest.Data.TrainingPrice == outerApiRequest.TrainingPrice) &&
            (innerApiRequest.Data.AssessmentPrice == outerApiRequest.AssessmentPrice);
    }
}