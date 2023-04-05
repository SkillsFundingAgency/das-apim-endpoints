using MediatR;

namespace SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;

public class CreateProviderCommand : IRequest<Unit>
{
    public string UserId { get; set; } = null!;
    public string UserDisplayName { get; set; } = null!;
    public int Ukprn { get; set; }
    public string LegalName { get; set; } = null!;
    public string? TradingName { get; set; }
}
