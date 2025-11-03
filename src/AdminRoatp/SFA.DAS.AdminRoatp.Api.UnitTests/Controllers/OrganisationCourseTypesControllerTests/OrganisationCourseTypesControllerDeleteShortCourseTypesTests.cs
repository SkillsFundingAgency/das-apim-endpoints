using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.DeleteOrganisationShortCourseTypes;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationCourseTypesControllerTests;
public class OrganisationCourseTypesControllerDeleteShortCourseTypesTests
{
    [Test, MoqAutoData]

    public async Task DeleteShortCourseTypes_InvokesHandler_VerifyHandlerIsCalled(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] OrganisationCourseTypesController sut,
        DeleteOrganisationShortCourseTypesCommand command)
    {
        // Arrange
        mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(HttpStatusCode.NoContent);

        // Act
        var result = await sut.DeleteShortCourseTypes(command.Ukprn, command.UserId, It.IsAny<CancellationToken>());

        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<DeleteOrganisationShortCourseTypesCommand>(r => r.Ukprn == command.Ukprn && r.UserId == command.UserId), It.IsAny<CancellationToken>()), Times.Once());
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.NoContent)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task DeleteShortCourseTypes_InvokesHandler_ReturnsInnerApiStatusCodes(HttpStatusCode expected)
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<OrganisationCourseTypesController>>();
        int ukprn = 12345678;
        string userId = "TestUser";
        var command = new DeleteOrganisationShortCourseTypesCommand(ukprn, userId);
        mediatorMock.Setup(m => m.Send(command, CancellationToken.None)).ReturnsAsync(expected);
        var sut = new OrganisationCourseTypesController(mediatorMock.Object, loggerMock.Object);

        // Act
        var result = await sut.DeleteShortCourseTypes(command.Ukprn, command.UserId, CancellationToken.None);

        // Assert
        result.As<StatusCodeResult>().StatusCode.Should().Be((int)expected);
    }
}