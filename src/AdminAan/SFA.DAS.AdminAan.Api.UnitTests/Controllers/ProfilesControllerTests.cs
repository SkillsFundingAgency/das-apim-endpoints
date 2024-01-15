using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;

public class ProfilesControllerTests
{
    [Test, AutoData]
    public async Task GetProfiles_InvokesInnerApi_ReturnsResult(string userType, GetProfilesResponse expected, CancellationToken cancellationToken)
    {
        Mock<IAanHubRestApiClient> clientMock = new();
        clientMock.Setup(c => c.GetProfiles(userType, cancellationToken)).ReturnsAsync(expected);
        ProfilesController sut = new(clientMock.Object);

        var actual = await sut.GetProfiles(userType, cancellationToken);

        clientMock.Verify(c => c.GetProfiles(userType, cancellationToken), Times.Once);

        actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
