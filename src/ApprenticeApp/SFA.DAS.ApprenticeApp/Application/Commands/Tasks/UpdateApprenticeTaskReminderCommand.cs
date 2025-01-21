using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Tasks
{
    public class UpdateApprenticeTaskReminderCommand : IRequest<Unit>
    {
        public int TaskId { get; set; }
        public int StatusId { get; set; }
    }
}
