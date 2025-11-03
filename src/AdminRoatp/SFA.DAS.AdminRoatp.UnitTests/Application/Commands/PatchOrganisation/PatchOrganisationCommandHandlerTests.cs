using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.PatchOrganisation;
public class PatchOrganisationCommandHandlerTests
{
    [Test]
    public async Task Handle_CallsApiClientPatchOrganisation()
    {
        // Arrange
        var roatpServiceApiClientMock = new Mock<IRoatpServiceRestApiClient>();
        var handler = new PatchOrganisationCommandHandler(roatpServiceApiClientMock.Object);
        int ukprn = 12345678;
        string userId = Guid.NewGuid().ToString();
        CancellationToken cancellationToken = new();
        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Main);
        roatpServiceApiClientMock.Setup(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken)).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));
        var command = new PatchOrganisationCommand(ukprn, userId, patchDoc);
        // Act
        await handler.Handle(command, cancellationToken);
        // Assert
        roatpServiceApiClientMock.Verify(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken), Times.Once);
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.NoContent)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task Handle_ReturnsResponseCodeAsReceivedFromInnerApi(HttpStatusCode expected)
    {
        // Arrange
        var roatpServiceApiClientMock = new Mock<IRoatpServiceRestApiClient>();
        int ukprn = 12345678;
        string userId = Guid.NewGuid().ToString();
        CancellationToken cancellationToken = new();
        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Main);
        var command = new PatchOrganisationCommand(ukprn, userId, patchDoc);

        roatpServiceApiClientMock.Setup(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken)).ReturnsAsync(new HttpResponseMessage(expected));

        var handler = new PatchOrganisationCommandHandler(roatpServiceApiClientMock.Object);
        // Act
        var actual = await handler.Handle(command, CancellationToken.None);
        // Assert
        actual.Should().Be(expected);
    }
}
