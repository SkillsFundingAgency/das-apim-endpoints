using AutoFixture.NUnit3;
using MediatR;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.Members.GetMemberProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers.MembersControllerTests;

public class MembersControllerGetMemberProfileTests
{
    [Test, MoqAutoData]
    public async Task GetMemberProfile_InvokesMediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MembersController sut,
        Guid memberId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        await sut.GetMemberProfile(memberId, requestedByMemberId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetMemberProfileQuery>(q => q.MemberId == memberId && q.RequestedByMemberId == requestedByMemberId), cancellationToken));
    }
}
