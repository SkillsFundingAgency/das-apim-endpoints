using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.UpdateMemberProfile;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Attendances.Commands.UpdateMember;
public class UpdateMemberCommandTests
{
    [Test, MoqAutoData]
    public void Constructor_InitialisesAllProperties(Guid memberId, Guid requestedByMemberId, UpdateMemberProfileRequest memberProfile)
    {
        var sut = new UpdateMemberCommand(memberId, requestedByMemberId, memberProfile);

        Assert.Multiple(() =>
        {
            Assert.That(sut.MemberId, Is.EqualTo(memberId));
            Assert.That(sut.RequestedByMemberId, Is.EqualTo(requestedByMemberId));
            Assert.That(sut.MemberProfile, Is.EqualTo(memberProfile));
        });
    }
}
