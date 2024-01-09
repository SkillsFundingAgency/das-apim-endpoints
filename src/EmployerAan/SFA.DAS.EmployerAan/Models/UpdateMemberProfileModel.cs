using SFA.DAS.EmployerAan.InnerApi.MemberProfiles;
using SFA.DAS.EmployerAan.InnerApi.Members;

namespace SFA.DAS.EmployerAan.Models;
public class UpdateMemberProfileModel
{
    public PatchMemberRequest patchMemberRequest { get; set; } = new PatchMemberRequest();
    public UpdateMemberProfileRequest updateMemberProfileRequest { get; set; } = new UpdateMemberProfileRequest();
}
