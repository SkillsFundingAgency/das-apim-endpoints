using MediatR;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles;
public class UpdateMemberProfilesCommand : IRequest<Unit>
{
    public Guid MemberId { get; set; }
    public Guid RequestedByMemberId { get; set; }
    public UpdateMemberProfileCommand MemberProfile { get; set; }

    public UpdateMemberProfilesCommand(Guid memberId, Guid requestedByMemberId, UpdateMemberProfileModel memberProfile)
    {
        MemberId = memberId;
        RequestedByMemberId = requestedByMemberId;
        MemberProfile = memberProfile;
    }
}
