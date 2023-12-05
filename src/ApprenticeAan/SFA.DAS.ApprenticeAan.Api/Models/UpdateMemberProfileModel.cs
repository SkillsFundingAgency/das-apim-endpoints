using SFA.DAS.ApprenticeAan.Application.InnerApi.MemberProfiles;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Members;

namespace SFA.DAS.ApprenticeAan.Api.Models;

public class UpdateMemberProfileModel
{
    public PatchMemberRequest patchMemberRequest { get; set; } = new PatchMemberRequest();
    public UpdateMemberProfileRequest updateMemberProfileRequest { get; set; } = new UpdateMemberProfileRequest();
}
