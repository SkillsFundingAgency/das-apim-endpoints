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

public class WhenCreateApprenticeshipPriceHistory
{
    private readonly Fixture _fixture;

    public WhenCreateApprenticeshipPriceHistory()
    {
		_fixture = new Fixture();
	}

    [Test]
    public async Task ThenCreatesApprenticeshipPriceHistoryUsingApiClient()
    {
        var apiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        var sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(), apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());
            
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
            
        apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostCreateApprenticeshipPriceChangeRequest>(), It.IsAny<bool>())).ReturnsAsync(new ApiResponse<object>("", HttpStatusCode.OK, ""));

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
        apiClient.Verify(x => x.PostWithResponseCode<object>(It.Is<PostCreateApprenticeshipPriceChangeRequest>(r =>
            ((CreateApprenticeshipPriceChangeRequest)r.Data).Initiator == ((CreateApprenticeshipPriceChangeRequest)request.Data).Initiator &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).UserId == ((CreateApprenticeshipPriceChangeRequest)request.Data).UserId &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).TrainingPrice == ((CreateApprenticeshipPriceChangeRequest)request.Data).TrainingPrice &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).AssessmentPrice == ((CreateApprenticeshipPriceChangeRequest)request.Data).AssessmentPrice &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).TotalPrice == ((CreateApprenticeshipPriceChangeRequest)request.Data).TotalPrice &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).Reason == ((CreateApprenticeshipPriceChangeRequest)request.Data).Reason &&
            ((CreateApprenticeshipPriceChangeRequest)r.Data).EffectiveFromDate == ((CreateApprenticeshipPriceChangeRequest)request.Data).EffectiveFromDate &&
            r.ApprenticeshipKey == request.ApprenticeshipKey), It.IsAny<bool>()), Times.Once);
        response.Should().BeOfType<OkResult>();
    }

	[Test]
	public async Task IfRequestFailsReturnsBadRequest()
	{
		var apiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
		var sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(), apiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());

		// Arrange
		var apprenticeshipKey = Guid.NewGuid();
		var request = _fixture.Create<Models.CreateApprenticeshipPriceChangeRequest>();
		apiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostCreateApprenticeshipPriceChangeRequest>(), It.IsAny<bool>())).ReturnsAsync(new ApiResponse<object>("", HttpStatusCode.NotFound, "Has Error"));

		// Act
		var response = await sut.CreateApprenticeshipPriceChange(apprenticeshipKey, request);

		// Assert
		response.Should().BeOfType<BadRequestResult>();
	}
}