using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Admins.Commands.Create;
public class CreateAdminMemberCommand : IRequest<CreateAdminMemberCommandResult>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
