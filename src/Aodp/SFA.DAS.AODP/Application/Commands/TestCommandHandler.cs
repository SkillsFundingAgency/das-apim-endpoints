using MediatR;

namespace SFA.DAS.AODP.Application.Commands;
public class TestCommandHandler : IRequestHandler<TestCommand>
{
    public async Task Handle(TestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}