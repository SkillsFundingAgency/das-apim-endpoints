using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Admins.Commands.Lookup;
public class LookupAdminMemberCommand : IRequest<LookupAdminMemberCommandResult?>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}