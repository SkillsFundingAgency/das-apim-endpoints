using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Members.PostMemberLeaving;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MembersControllerTests;
public class PostMembersLeavingControllerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task PostMemberLeaving_InvokesPostMemberLeaving_NoContent(
        PostMemberLeavingModel request,
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        aanHubRestApiClientMock.Setup(x => x.PostMembersLeaving(memberId, request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RestEase.Response<PostMemberLeavingResponse>(null, new HttpResponseMessage(HttpStatusCode.NoContent), () => new PostMemberLeavingResponse()));

        var result = await sut.PostMemberLeavingReasons(memberId, request, cancellationToken);
        aanHubRestApiClientMock.Verify(x => x.PostMembersLeaving(memberId, request, It.IsAny<CancellationToken>()), Times.Once());

        result.As<NoContentResult>().Should().NotBeNull();
    }

    [Test, RecursiveMoqAutoData]
    public async Task PostMemberLeaving_InvokesPostMemberLeaving_NotFound(
        PostMemberLeavingModel request,
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        aanHubRestApiClientMock.Setup(x => x.PostMembersLeaving(memberId, request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RestEase.Response<PostMemberLeavingResponse>(null, new HttpResponseMessage(HttpStatusCode.NotFound), () => new PostMemberLeavingResponse()));

        var result = await sut.PostMemberLeavingReasons(memberId, request, cancellationToken);
        aanHubRestApiClientMock.Verify(x => x.PostMembersLeaving(memberId, request, It.IsAny<CancellationToken>()), Times.Once());

        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public void PostMemberLeaving_InvokesPostMemberLeaving_ThrowInvalidOperationException(
        PostMemberLeavingModel request,
        [Frozen] Mock<IAanHubRestApiClient> aanHubRestApiClientMock,
        [Greedy] MembersController sut,
        Guid memberId,
        CancellationToken cancellationToken)
    {
        RestEase.Response<PostMemberLeavingResponse> response = new(null, new HttpResponseMessage(HttpStatusCode.BadRequest), () => new PostMemberLeavingResponse());

        aanHubRestApiClientMock.Setup(x => x.PostMembersLeaving(memberId, request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        Assert.That(() => sut.PostMemberLeavingReasons(memberId, request, cancellationToken), Throws.InvalidOperationException);
    }
}
