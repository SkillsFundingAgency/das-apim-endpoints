using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.DeleteOrganisationShortCourseTypes;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.DeleteOrganisationShortCourseTypes;
public class DeleteOrganisationShortCourseTypesCommandHandlerTests
{
    [Test, MoqAutoData]

    public async Task Handle_CallsApiClient_VerifyDeleteShortCourseTypesIsCalled(
        [Frozen] Mock<IRoatpServiceRestApiClient> apiClientMock,
        DeleteOrganisationShortCourseTypesCommandHandler sut,
        DeleteOrganisationShortCourseTypesCommand command)
    {
        // Arrange
        apiClientMock.Setup(a => a.DeleteShortCourseTypes(command.Ukprn, command.UserId, CancellationToken.None)).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        apiClientMock.Verify(a => a.DeleteShortCourseTypes(command.Ukprn, command.UserId, CancellationToken.None), Times.Once);
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.NoContent)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task Handle_CallsApiClient_ReturnsInnerApiStatusCodes(HttpStatusCode expected)
    {
        // Arrange
        var apiClientMock = new Mock<IRoatpServiceRestApiClient>();
        int ukprn = 12345678;
        string userId = "TestUser";
        var command = new DeleteOrganisationShortCourseTypesCommand(ukprn, userId);
        apiClientMock.Setup(a => a.DeleteShortCourseTypes(command.Ukprn, command.UserId, CancellationToken.None)).ReturnsAsync(new HttpResponseMessage(expected));
        var sut = new DeleteOrganisationShortCourseTypesCommandHandler(apiClientMock.Object);

        // Act
        var result = await sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }
}