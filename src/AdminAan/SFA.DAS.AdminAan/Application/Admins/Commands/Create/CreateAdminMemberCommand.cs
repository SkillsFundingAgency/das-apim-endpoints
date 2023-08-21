using MediatR;
using SFA.DAS.AdminAan.Application.Admins.Queries.Lookup;

namespace SFA.DAS.AdminAan.Application.Admins.Commands.Create;
public class CreateAdminMemberCommand : IRequest<CreateAdminMemberCommandResult>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;


    public static implicit operator CreateAdminMemberCommand(LookupAdminMemberRequest requestModel) => new()
    {
        Email = requestModel.Email,
        FirstName = requestModel.FirstName,
        LastName = requestModel.LastName
    };
}