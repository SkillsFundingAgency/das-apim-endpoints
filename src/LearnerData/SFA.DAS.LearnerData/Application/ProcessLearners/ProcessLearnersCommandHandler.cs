using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Application.ProcessLearners;

public class ProcessLearnersCommandHandler(
    ILogger<ProcessLearnersCommandHandler> logger,
    IMessageSession messageSession) : IRequestHandler<ProcessLearnersCommand>
{
    public Task Handle(ProcessLearnersCommand request, CancellationToken cancellationToken)
    {
        return Parallel.ForEachAsync(request.Learners, new ParallelOptions { MaxDegreeOfParallelism = 5 },
            async (learner, cancellationToken) =>
            {
                logger.LogTrace("Publishing LearnerDataEvent");
                var evt = MapToEvent(learner, request.CorrelationId, request.ReceivedOn, request.AcademicYear);
                await messageSession.Publish(evt);
            });
    }

    private LearnerDataEvent MapToEvent(LearnerDataRequest request, Guid correlationId, DateTime receivedOn, int academicYear)
    {
        return new LearnerDataEvent
        {
            ULN = request.ULN,
            UKPRN = request.UKPRN,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.LearnerEmail,
            DoB = request.DateOfBirth,
            StartDate = request.StartDate,
            PlannedEndDate = request.PlannedEndDate,
            PercentageLearningToBeDelivered = request.PercentageLearningToBeDelivered,
            EpaoPrice = request.EpaoPrice,
            TrainingPrice = request.TrainingPrice,
            AgreementId = request.AgreementId,
            IsFlexiJob = request.IsFlexiJob,
            PlannedOTJTrainingHours = request.PlannedOTJTrainingHours ?? 0,
            StandardCode = request.StandardCode,
            CorrelationId = correlationId,
            ReceivedDate = receivedOn,
            AcademicYear = academicYear,
            ConsumerReference = request.ConsumerReference
        };
    }
}