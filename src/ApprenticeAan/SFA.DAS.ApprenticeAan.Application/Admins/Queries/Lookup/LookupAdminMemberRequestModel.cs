using MediatR;

namespace SFA.DAS.ApprenticeAan.Application.Admins.Queries.Lookup;

public class LookupAdminMemberRequestModel : IRequest<LookupAdminMemberResult?>
{
    public string Email { get; set; } = null!;

    public static implicit operator LookupAdminMemberRequestModel(LookupAdminMemberRequest requestModel) => new()
    {
        Email = requestModel.Email
    };

}