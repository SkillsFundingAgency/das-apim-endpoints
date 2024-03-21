using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.GetAdminMember;

namespace SFA.DAS.AdminAan.Api.Models.Admins;
public class GetAdminMemberRequestModel
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public static implicit operator GetAdminMemberRequest(GetAdminMemberRequestModel requestModel) => new()
    {
        Email = requestModel.Email
    };

    public static implicit operator CreateAdminMemberCommand(GetAdminMemberRequestModel requestModel) => new()
    {
        Email = requestModel.Email,
        FirstName = requestModel.FirstName,
        LastName = requestModel.LastName
    };
}