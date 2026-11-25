using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderNotRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderRestrictedCoursesControllerTests;

public class ProviderRestrictedCoursesControllerGetTests
{
    [Test, MoqAutoData]
    public async Task WhenGetProviderRestrictedApprenticeshipsIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderRestrictedCoursesController sut,
        GetProviderRestrictedApprenticeshipsResponse response,
        int ukprn)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProviderRestrictedApprenticeshipsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetProviderRestrictedApprenticeships(ukprn, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderRestrictedApprenticeshipsIsInvoked_ThenMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderRestrictedCoursesController sut,
        GetProviderRestrictedApprenticeshipsResponse response,
        int ukprn)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.Is<GetProviderRestrictedApprenticeshipsQuery>(q => q.Ukprn == ukprn), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await sut.GetProviderRestrictedApprenticeships(ukprn, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<GetProviderRestrictedApprenticeshipsQuery>(q => q.Ukprn == ukprn), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderNotRestrictedApprenticeshipsIsInvoked_ThenReturnsOkResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderRestrictedCoursesController sut,
        GetProviderNotRestrictedApprenticeshipsResponse response,
        int ukprn)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetProviderNotRestrictedApprenticeshipsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetProviderNotRestrictedApprenticeships(ukprn, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(response);
    }

    [Test, MoqAutoData]
    public async Task WhenGetProviderNotRestrictedApprenticeshipsIsInvoked_ThenMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderRestrictedCoursesController sut,
        GetProviderNotRestrictedApprenticeshipsResponse response,
        int ukprn)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.Is<GetProviderNotRestrictedApprenticeshipsQuery>(q => q.Ukprn == ukprn), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await sut.GetProviderNotRestrictedApprenticeships(ukprn, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<GetProviderNotRestrictedApprenticeshipsQuery>(q => q.Ukprn == ukprn), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
