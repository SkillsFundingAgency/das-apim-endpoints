using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Members.PostMemberLeaving;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MembersControllerTests;
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
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));

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
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NotFound));

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
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

        aanHubRestApiClientMock.Setup(x => x.PostMembersLeaving(memberId, request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        Assert.That(() => sut.PostMemberLeavingReasons(memberId, request, cancellationToken), Throws.InvalidOperationException);
    }
}
