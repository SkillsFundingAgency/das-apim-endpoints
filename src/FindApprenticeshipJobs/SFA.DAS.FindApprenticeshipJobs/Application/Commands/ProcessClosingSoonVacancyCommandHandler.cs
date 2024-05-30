using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessClosingSoonVacancyCommandHandler : IRequestHandler<ProcessClosingSoonVacancyCommand, Unit>
{
    public async Task<Unit> Handle(ProcessClosingSoonVacancyCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}