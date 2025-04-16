using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;

namespace SFA.DAS.LearnerData.Application;

public class ProcessLearnersCommandHandler(ILogger<ProcessLearnersCommandHandler> logger, IMessageSession messageSession) : IRequestHandler<ProcessLearnersCommand>
{
    public Task Handle(ProcessLearnersCommand request, CancellationToken cancellationToken)
    {
        return Parallel.ForEachAsync(request.Learners, new ParallelOptions {MaxDegreeOfParallelism = 5},
            async (learner, cancellationToken) =>
            {
                logger.LogTrace("Publishing LearnerDataEvent");
                var evt = (LearnerDataEvent) learner;
                await messageSession.Publish(evt);
            });
    }
}