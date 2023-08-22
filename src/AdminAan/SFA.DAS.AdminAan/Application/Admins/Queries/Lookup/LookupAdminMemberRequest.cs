using MediatR;

namespace SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;
public class LookupAdminMemberRequest : IRequest<LookupAdminMemberResult?>
{
    public string Email { get; set; } = null!;
}
