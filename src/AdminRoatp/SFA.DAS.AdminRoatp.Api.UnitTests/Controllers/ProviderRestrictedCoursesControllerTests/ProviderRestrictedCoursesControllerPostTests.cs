using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateProviderRestrictedApprenticeship;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderRestrictedCoursesControllerTests;

public class ProviderRestrictedCoursesControllerPostTests
{
    [Test, MoqAutoData]
    public async Task WhenUpdateProviderRestrictedApprenticeshipIsInvoked_ThenReturnsNoContent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderRestrictedCoursesController sut,
        UpdateProviderRestrictedApprenticeshipModel request,
        int ukprn,
        string larsCode)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateProviderRestrictedApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.UpdateProviderRestrictedApprenticeship(ukprn, larsCode, request, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test, MoqAutoData]
    public async Task WhenUpdateProviderRestrictedApprenticeshipIsInvoked_ThenMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderRestrictedCoursesController sut,
        UpdateProviderRestrictedApprenticeshipModel request,
        int ukprn,
        string larsCode)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateProviderRestrictedApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await sut.UpdateProviderRestrictedApprenticeship(ukprn, larsCode, request, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<UpdateProviderRestrictedApprenticeshipCommand>(c =>
                c.Ukprn == ukprn &&
                c.LarsCode == larsCode &&
                c.UserId == request.UserId &&
                c.UserDisplayName == request.UserDisplayName &&
                c.LastDateStarts == request.LastDateStarts), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
