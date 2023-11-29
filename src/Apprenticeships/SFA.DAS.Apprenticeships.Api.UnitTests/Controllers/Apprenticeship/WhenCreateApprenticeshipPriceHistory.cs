using System;
using System.Net;
using System.Threading.Tasks;
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
    [Test]
    public async Task ThenCreatesApprenticeshipPriceHistoryUsingApiClient()
    {
        var apiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        var sut = new ApprenticeshipController(apiClient.Object);
            
        // Arrange
        var apprenticeshipKey = Guid.NewGuid();
        var request = new CreateApprenticeshipPriceChangeRequest
        {
            ApprenticeshipKey = apprenticeshipKey,
            Data = new CreateApprenticeshipPriceChangeRequestData
            {
                Ukprn = 123,
                EmployerId = 456,
                UserId = "testUser",
                TrainingPrice = 1000,
                AssessmentPrice = 500,
                TotalPrice = 1500,
                Reason = "Test Reason"
            }
        };
            
        apiClient.Setup(x => x.PostWithResponseCode<object>(request, It.IsAny<bool>())).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.OK, ""));

        // Act
        await sut.CreateApprenticeshipPriceChange(
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).Ukprn,
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).EmployerId,
            request.ApprenticeshipKey,
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).UserId,
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).TrainingPrice,
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).AssessmentPrice,
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).TotalPrice,
            ((CreateApprenticeshipPriceChangeRequestData)request.Data).Reason);

        // Assert
        apiClient.Verify(x => x.PostWithResponseCode<object>(It.Is<CreateApprenticeshipPriceChangeRequest>(r =>
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).Ukprn == ((CreateApprenticeshipPriceChangeRequestData)request.Data).Ukprn &&
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).EmployerId == ((CreateApprenticeshipPriceChangeRequestData)request.Data).EmployerId &&
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).UserId == ((CreateApprenticeshipPriceChangeRequestData)request.Data).UserId &&
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).TrainingPrice == ((CreateApprenticeshipPriceChangeRequestData)request.Data).TrainingPrice &&
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).AssessmentPrice == ((CreateApprenticeshipPriceChangeRequestData)request.Data).AssessmentPrice &&
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).TotalPrice == ((CreateApprenticeshipPriceChangeRequestData)request.Data).TotalPrice &&
            ((CreateApprenticeshipPriceChangeRequestData)r.Data).Reason == ((CreateApprenticeshipPriceChangeRequestData)request.Data).Reason &&
            r.ApprenticeshipKey == request.ApprenticeshipKey), It.IsAny<bool>()), Times.Once);
    }
}