using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Domain.LeavingReasons;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers.MembersControllerTests;
public class MembersControllerPostMemberLeavingTest
{
    private Guid memberId = Guid.NewGuid();
    private Mock<IAanHubRestApiClient> apiClientMock = null!;
    private MembersController sut = null!;

    [Test, AutoData]
    public async Task PostMemberLeaving_ForwardsRequestToInnerApi(
        PostMemberStatusModel postMemberStatusModel,
        string expected)
    {
        // Arrange
        apiClientMock = new();
        apiClientMock.Setup(m => m.PostMemberLeaving(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PostMemberStatusModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);
        sut = new(apiClientMock.Object, Mock.Of<IMediator>());

        // Act
        var actual = await sut.PostMemberLeaving(memberId, memberId, postMemberStatusModel, CancellationToken.None);

        // Assert
        apiClientMock.Verify(m => m.PostMemberLeaving(memberId, memberId, postMemberStatusModel, CancellationToken.None));
    }

    [Test, AutoData]
    public async Task PostMemberLeaving_ReturnsExpectedResponse(
        PostMemberStatusModel postMemberStatusModel,
        string expected)
    {
        // Arrange
        apiClientMock = new();
        apiClientMock.Setup(m => m.PostMemberLeaving(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PostMemberStatusModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);
        sut = new(apiClientMock.Object, Mock.Of<IMediator>());

        // Act
        var result = await sut.PostMemberLeaving(memberId, memberId, postMemberStatusModel, CancellationToken.None);

        // Assert
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
