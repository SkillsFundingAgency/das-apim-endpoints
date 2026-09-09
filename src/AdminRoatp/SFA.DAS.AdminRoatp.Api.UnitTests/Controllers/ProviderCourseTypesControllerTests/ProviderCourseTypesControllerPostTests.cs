using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.RestrictProvider;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.ProviderCourseTypesControllerTests;

public class ProviderCourseTypesControllerPostTests
{
    [Test, MoqAutoData]
    public async Task WhenRestrictProviderIsInvoked_ThenReturnsNoContentResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCourseTypesController sut,
        RestrictProviderModel request,
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(
                It.IsAny<RestrictProviderCommand>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await sut.RestrictProvider(ukprn, courseType, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test, MoqAutoData]
    public async Task WhenRestrictProviderIsInvoked_ThenVerifyMediatorIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCourseTypesController sut,
        RestrictProviderModel request,
        int ukprn,
        CourseType courseType)
    {
        // Arrange
        mediatorMock
            .Setup(x => x.Send(
                It.Is<RestrictProviderCommand>(c =>
                    c.Ukprn == ukprn &&
                    c.CourseType == courseType &&
                    c.UserId == request.UserId &&
                    c.UserDisplayName == request.UserDisplayName),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await sut.RestrictProvider(ukprn, courseType, request);

        // Assert
        mediatorMock.Verify(x => x.Send(
            It.Is<RestrictProviderCommand>(c =>
                c.Ukprn == ukprn &&
                c.CourseType == courseType &&
                c.UserId == request.UserId &&
                c.UserDisplayName == request.UserDisplayName),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
