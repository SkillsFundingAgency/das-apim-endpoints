using MediatR;

namespace SFA.DAS.RoatpOversight.Application.Providers.Command.CreateProvider;

public class CreateProviderCommandHandler : IRequestHandler<CreateProviderCommand, Unit>
{
    public Task<Unit> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
