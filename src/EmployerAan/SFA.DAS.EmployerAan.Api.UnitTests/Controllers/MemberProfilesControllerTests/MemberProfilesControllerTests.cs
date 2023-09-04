using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.EmployerAan.InnerApi.MemberProfiles;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.MemberProfilesControllerTests;
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

        await sut.PutMemberProfile(memberId, requestedByMemberId, request, cancellationToken);

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
        var sut = new MemberProfilesController(mediatorMock.Object);

        var result = await sut.PutMemberProfile(memberId, requestedByMemberId, request, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}
