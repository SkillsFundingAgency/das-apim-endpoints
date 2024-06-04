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
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship;

public class WhenFreezePayments
{
    private readonly Fixture _fixture;
    private readonly Mock<ILogger<ApprenticeshipController>> _mockLogger;
    private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApiClient;

    public WhenFreezePayments()
    {
        _fixture = new Fixture();
        _mockLogger = new Mock<ILogger<ApprenticeshipController>>();
        _mockApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
    }

    [SetUp]
    public void Arrange()
    {
        _mockApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
    }

    [Test]
    public async Task ThenSendsPostRequest()
    {
        // Arrange
        var sut = new ApprenticeshipController(_mockLogger.Object, _mockApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<FreezePaymentsRequest>();

        _mockApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>(), false))
            .ReturnsAsync(new ApiResponse<object>("", HttpStatusCode.OK, ""));

        // Act
        var response = await sut.FreezeApprenticeshipPayments(apprenticeshipKey, request);

        // Assert
        _mockApiClient.Verify(x => x.PostWithResponseCode<object>(It.Is<PostFreezePaymentsRequest>(y =>
            y.ApprenticeshipKey == apprenticeshipKey), false), Times.Once);

        response.Should().BeOfType<OkResult>();
    }

    [Test]
    public async Task IfRequestFailsReturnsBadRequest()
    {
        // Arrange
        var sut = new ApprenticeshipController(_mockLogger.Object, _mockApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), Mock.Of<IMediator>());

        var apprenticeshipKey = Guid.NewGuid();
        var request = _fixture.Create<FreezePaymentsRequest>();

        _mockApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>(), false))
            .ReturnsAsync(new ApiResponse<object>("", HttpStatusCode.NotFound, "Has Error"));

        // Act
        var response = await sut.FreezeApprenticeshipPayments(apprenticeshipKey, request);

        // Assert
        response.Should().BeOfType<BadRequestResult>();
    }
}