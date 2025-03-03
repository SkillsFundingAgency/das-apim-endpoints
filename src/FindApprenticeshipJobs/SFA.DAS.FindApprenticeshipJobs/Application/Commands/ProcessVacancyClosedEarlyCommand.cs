using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessVacancyClosedEarlyCommand : IRequest<Unit>
{
    public long VacancyReference { get; set; }
}