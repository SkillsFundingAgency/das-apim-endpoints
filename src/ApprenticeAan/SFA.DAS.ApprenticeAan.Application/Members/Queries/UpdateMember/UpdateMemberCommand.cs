using MediatR;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;

namespace SFA.DAS.ApprenticeAan.Application.MemberProfiles.Queries.UpdateMemberProfile;
public class UpdateMemberCommand : IRequest<Unit>
{
    public Guid MemberId { get; set; }
    public Guid RequestedByMemberId { get; set; }
    public UpdateMemberProfileRequest MemberProfile { get; set; }

    public UpdateMemberCommand(Guid memberId, Guid requestedByMemberId, UpdateMemberProfileRequest memberProfile)
    {
        MemberId = memberId;
        RequestedByMemberId = requestedByMemberId;
        MemberProfile = memberProfile;
    }
}
