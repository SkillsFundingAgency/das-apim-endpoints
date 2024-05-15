using MediatR;

namespace SFA.DAS.AdminAan.Application.Admins.Queries.GetAdminMember;
public class GetAdminMemberRequest : IRequest<GetAdminMemberResult?>
{
    public string Email { get; set; } = null!;
}
