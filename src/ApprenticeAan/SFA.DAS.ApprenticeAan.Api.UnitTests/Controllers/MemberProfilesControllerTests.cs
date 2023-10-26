using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;
public class MemberProfilesControllerTests
{
    [Test, MoqAutoData]
    public async Task PutMemberProfile_InvokesSendWithCommandProperties(
        [Frozen] Mock<IMediator> mediatorMock,
        Guid memberId,
        Guid requestedByMemberId,
        UpdateMemberProfileModel request,
        CancellationToken cancellationToken)
    {
        var sut = new MemberProfilesController(mediatorMock.Object);

        var model = new UpdateMemberProfileModel
        {
            Model = request
        };
        await sut.PutMemberProfile(memberId, requestedByMemberId, model, cancellationToken);

        mediatorMock.Verify(m => m.Send(
            It.Is<UpdateMemberProfilesCommand>(
                c => c.MemberId == memberId
                && c.RequestedByMemberId == requestedByMemberId
                && c.MemberProfile == request), cancellationToken),
                     Times.Once());
    }

    [Test, MoqAutoData]
    public async Task PutMemberProfile_ReturnsNoContent(
    [Frozen] Mock<IMediator> mediatorMock,
    Guid memberId,
    Guid requestedByMemberId,
    UpdateMemberProfileModel request,
    CancellationToken cancellationToken)
    {
        var model = new UpdateMemberProfileModel
        {
            Model = request
        };
        var sut = new MemberProfilesController(mediatorMock.Object);

        var result = await sut.PutMemberProfile(memberId, requestedByMemberId, model, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}
