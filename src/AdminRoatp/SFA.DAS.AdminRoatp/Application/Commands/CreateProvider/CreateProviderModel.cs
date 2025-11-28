using MediatR;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;

namespace SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
public class CreateProviderModel : IRequest<Unit>
{
    public string UserId { get; set; } = null!;
    public string UserDisplayName { get; set; } = null!;
    public int Ukprn { get; set; }
    public string LegalName { get; set; } = null!;
    public string? TradingName { get; set; }

    public static implicit operator CreateProviderModel(PostOrganisationCommand source) =>
        new()
        {
            Ukprn = source.Ukprn,
            LegalName = source.LegalName,
            TradingName = source.TradingName,
            UserId = source.RequestingUserId,
            UserDisplayName = source.RequestingUserDisplayName,
        };
}
