using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MemberProfiles;
public class UpdateMemberProfilesCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        UpdateMemberProfilesCommandHandler sut,
        UpdateMemberProfilesCommand command,
        CancellationToken token)
    {
        await sut.Handle(command, token);

        apiClientMock.Verify(c => c.PutMemberProfile(command.MemberId, command.RequestedByMemberId, command.MemberProfile, token), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_AlwaysReturnsUnitValue(
        UpdateMemberProfilesCommandHandler sut,
        UpdateMemberProfilesCommand command,
        CancellationToken token)
    {
        var result = await sut.Handle(command, token);
        result.Should().Be(Unit.Value);
    }
}
