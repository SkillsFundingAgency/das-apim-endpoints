using MediatR;

namespace SFA.DAS.TrackProgress.Apis.Commitments.Commands;

public static class TrackApprenticeProgress
{
    public class Command : IRequest
    {
        public long Ukprn { get; set; }
        public long Uln { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}