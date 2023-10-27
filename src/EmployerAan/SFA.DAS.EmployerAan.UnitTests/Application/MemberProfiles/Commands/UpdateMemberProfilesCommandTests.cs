using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.EmployerAan.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.EmployerAan.InnerApi.MemberProfiles;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.MemberProfiles.Commands;
public class UpdateMemberProfilesCommandTests
{
    [Test, MoqAutoData]
    public void Constructor_InitialisesAllProperties(Guid memberId, Guid requestedByMemberId, UpdateMemberProfileModel memberProfile)
    {
        var sut = new UpdateMemberProfilesCommand(memberId, requestedByMemberId, memberProfile);

        using (new AssertionScope())
        {
            sut.MemberId.Should().Be(memberId);
            sut.RequestedByMemberId.Should().Be(requestedByMemberId);
            sut.MemberProfile.Should().BeEquivalentTo((UpdateMemberProfileCommand)memberProfile);
        }
    }
}
