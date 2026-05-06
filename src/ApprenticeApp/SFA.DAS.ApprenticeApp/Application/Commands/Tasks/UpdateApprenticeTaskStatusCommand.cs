using MediatR;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class UpdateApprenticeTaskStatusCommand : IRequest<Unit>
    {
        public long ApprenticeshipId { get; set; }
        public int TaskId { get; set; }
        public int StatusId { get; set; }
    }
}