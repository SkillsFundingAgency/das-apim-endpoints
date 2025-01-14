using MediatR;
using SFA.DAS.AODP.Application.Commands;

namespace SFA.DAS.ReferenceDataJobs.Application.Commands;
public class TestCommandHandler : IRequestHandler<TestCommand>
{
    public async Task Handle(TestCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}