using SFA.DAS.AdminAan.Application.Admins.Commands.Create;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;

namespace SFA.DAS.AdminAan.Api.Models.Admins;
public class LookupAdminMemberRequestModel
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public static implicit operator LookupAdminMemberRequest(LookupAdminMemberRequestModel requestModelModel) => new()
    {
        Email = requestModelModel.Email
    };

    public static implicit operator CreateAdminMemberCommand(LookupAdminMemberRequestModel requestModelModel) => new()
    {
        Email = requestModelModel.Email,
        FirstName = requestModelModel.FirstName,
        LastName = requestModelModel.LastName
    };
}