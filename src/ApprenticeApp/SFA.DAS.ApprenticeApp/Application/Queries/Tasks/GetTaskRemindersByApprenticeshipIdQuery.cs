using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Tasks
{
    public class GetTaskRemindersByApprenticeshipIdQuery : IRequest<GetTaskRemindersByApprenticeshipIdQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }
}
