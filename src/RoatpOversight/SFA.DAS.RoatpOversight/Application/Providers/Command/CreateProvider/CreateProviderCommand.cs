using MediatR;

namespace SFA.DAS.RoatpOversight.Application.Providers.Command.CreateProvider;

public class CreateProviderCommand : IRequest<Unit>
{
    public string RequestingUserId { get; set; } = null!;
    public int Ukprn { get; set; }
    public string Name { get; set; } = null!;
}
