using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MembersControllerTests;
public class PostMembersReinstateControllerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task PostMemberReinstate_InvokesPostMemberReinstate_NoContent(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        aanHubRestApiClientMock.Setup(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

        var result = await sut.PostMemberReinstate(memberId, cancellationToken);
        aanHubRestApiClientMock.Verify(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()), Times.Once());

        result.As<NoContentResult>().Should().NotBeNull();
    }

    [Test, RecursiveMoqAutoData]
    public async Task PostMemberLeaving_InvokesPostMemberLeaving_NotFound(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        aanHubRestApiClientMock.Setup(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

        var result = await sut.PostMemberReinstate(memberId, cancellationToken);
        aanHubRestApiClientMock.Verify(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()), Times.Once());

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, RecursiveMoqAutoData]
    public async Task PostMemberLeaving_InvokesPostMemberLeaving_BadRequest(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        aanHubRestApiClientMock.Setup(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest));

        var result = await sut.PostMemberReinstate(memberId, cancellationToken);
        aanHubRestApiClientMock.Verify(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()), Times.Once());

        result.As<BadRequestResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public void PostMemberLeaving_InvokesPostMemberLeaving_ThrowInvalidOperationException(
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Ambiguous);

        aanHubRestApiClientMock.Setup(x => x.PostMembersReinstate(memberId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        Assert.That(() => sut.PostMemberReinstate(memberId, cancellationToken), Throws.InvalidOperationException);
    }
}