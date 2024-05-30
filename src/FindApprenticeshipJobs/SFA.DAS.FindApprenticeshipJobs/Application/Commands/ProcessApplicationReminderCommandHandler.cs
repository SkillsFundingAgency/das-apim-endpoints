using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessApplicationReminderCommandHandler : IRequestHandler<ProcessApplicationReminderCommand, Unit>
{
    public async Task<Unit> Handle(ProcessApplicationReminderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}