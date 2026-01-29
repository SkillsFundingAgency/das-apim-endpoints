using System.Net;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.AdminRoatp.Api.Controllers;
using SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.Api.UnitTests.Controllers.OrganisationsControllerTests;
public class OrganisationsControllerPatchOrganisationTests
{
    [Test]
    public async Task PatchOrganisation_InvokesHandler()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<OrganisationsController>>();
        var controller = new OrganisationsController(mediatorMock.Object, loggerMock.Object);
        int ukprn = 12345678;
        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Main);
        string userId = Guid.NewGuid().ToString();
        string userName = Guid.NewGuid().ToString();
        mediatorMock.Setup(m => m.Send(It.IsAny<PatchOrganisationCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(HttpStatusCode.NoContent);
        // Act
        await controller.PatchOrganisation(ukprn, patchDoc, userId, userName, CancellationToken.None);
        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<PatchOrganisationCommand>(cmd =>
            cmd.Ukprn == ukprn &&
            cmd.UserId == userId &&
            cmd.PatchDoc == patchDoc
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.NoContent)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task PatchOrganisation_InvokesHandler(HttpStatusCode expected)
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<OrganisationsController>>();
        int ukprn = 12345678;
        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Main);
        string userId = Guid.NewGuid().ToString();
        string userName = Guid.NewGuid().ToString();

        mediatorMock.Setup(m => m.Send(It.Is<PatchOrganisationCommand>(cmd =>
            cmd.Ukprn == ukprn &&
            cmd.UserId == userId &&
            cmd.PatchDoc == patchDoc
        ), It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var controller = new OrganisationsController(mediatorMock.Object, loggerMock.Object);
        // Act
        var result = await controller.PatchOrganisation(ukprn, patchDoc, userId, userName, CancellationToken.None);
        // Assert
        result.As<StatusCodeResult>().StatusCode.Should().Be((int)expected);
    }
}
