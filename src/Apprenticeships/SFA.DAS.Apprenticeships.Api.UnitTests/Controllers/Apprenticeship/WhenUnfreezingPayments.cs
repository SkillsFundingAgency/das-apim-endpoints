using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
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

#pragma warning disable CS8618
public class WhenUnfreezingPayments
{
    private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient = null!;
    private ApprenticeshipController _sut = null!;
    private Fixture _fixture = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
    }

    [Test]
    public async Task IfErrorReturnedFromApiRequest_ThenReturnBadRequest()
    {
        //  Arrange
        SetUpControllerWithErrorResponseFromInnerApi();
        var apprenticeshipKey = _fixture.Create<Guid>();

        //  Act
        var result = await _sut.UnfreezeApprenticeshipPayments(apprenticeshipKey);

        //  Assert
        result.ShouldBeOfType<BadRequestResult>();
    }

    [Test]
    public async Task IfNoErrorReturnedFromApiRequest_ThenReturnOk()
    {
        //  Arrange
        SetUpController();
        var apprenticeshipKey = _fixture.Create<Guid>();

        //  Act
        var result = await _sut.UnfreezeApprenticeshipPayments(apprenticeshipKey);

        //  Assert
        _mockApprenticeshipsApiClient.Verify(x =>
            x.PostWithResponseCode<object>(It.Is<PostUnfreezePaymentsRequest>(r => r.ApprenticeshipKey == apprenticeshipKey), false), Times.Once);
        result.ShouldBeOfType<OkResult>();
    }

    private void SetUpController()
    {
        var basicApiResponse = new ApiResponse<object>("", HttpStatusCode.OK, "");
        _mockApprenticeshipsApiClient
            .Setup(apiClient => apiClient.PostWithResponseCode<object>(It.IsAny<PostUnfreezePaymentsRequest>(), false))
            .ReturnsAsync(basicApiResponse);
        _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(),
            _mockApprenticeshipsApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(),
            Mock.Of<IMediator>());
    }

    private void SetUpControllerWithErrorResponseFromInnerApi()
    {
        var errorResponse = new ApiResponse<object>("", HttpStatusCode.InternalServerError, "errorResponse");
        _mockApprenticeshipsApiClient
            .Setup(apiClient => apiClient.PostWithResponseCode<object>(It.IsAny<PostUnfreezePaymentsRequest>(), false))
            .ReturnsAsync(errorResponse);
        _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(),
            _mockApprenticeshipsApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(),
            Mock.Of<IMediator>());
    }
}