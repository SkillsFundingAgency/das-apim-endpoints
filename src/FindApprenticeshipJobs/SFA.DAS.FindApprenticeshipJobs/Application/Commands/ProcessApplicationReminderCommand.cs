using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands;

public class ProcessApplicationReminderCommand : IRequest<Unit>
{
    public int DaysUntilClosing { get; set; }
    public long VacancyReference { get; set; }
}