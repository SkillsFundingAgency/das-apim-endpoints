using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;
public class LookupAdminMemberRequest : IRequest<LookupAdminMemberResult?>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}