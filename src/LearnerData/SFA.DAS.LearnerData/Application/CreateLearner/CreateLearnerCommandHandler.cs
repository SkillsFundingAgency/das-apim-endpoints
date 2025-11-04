using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;

namespace SFA.DAS.LearnerData.Application.CreateLearner;

public class CreateLearnerCommandHandler(
    ILogger<CreateLearnerCommandHandler> logger,
    IMessageSession messageSession) : IRequestHandler<CreateLearnerCommand>
{
    public async Task Handle(CreateLearnerCommand command, CancellationToken cancellationToken)
    {
        logger.LogTrace("Publishing LearnerDataEvent");
        var evt = MapToEvent(command);
        await messageSession.Publish(evt);
    }

    private LearnerDataEvent MapToEvent(CreateLearnerCommand command)
    {
        var cost = command.Request.Delivery.OnProgramme.MapCosts().First();

        return new LearnerDataEvent
        {
            ULN = long.Parse(command.Request.Learner.Uln),
            UKPRN = command.Ukprn,
            FirstName = command.Request.Learner.Firstname,
            LastName = command.Request.Learner.Lastname,
            Email = command.Request.Learner.Email,
            DoB = command.Request.Learner.Dob!.Value,
            StartDate = command.Request.Delivery.OnProgramme.StartDate!.Value,
            PlannedEndDate = command.Request.Delivery.OnProgramme.ExpectedEndDate,
            PercentageLearningToBeDelivered = command.Request.Delivery.OnProgramme.PercentageOfTrainingLeft,
            EpaoPrice = cost.EpaoPrice ?? 0,
            TrainingPrice = cost.TrainingPrice,
            AgreementId = command.Request.Delivery.OnProgramme.AgreementId,
            IsFlexiJob = command.Request.Delivery.OnProgramme.IsFlexiJob!.Value,
            StandardCode = command.Request.Delivery.OnProgramme.StandardCode!.Value,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = command.Request.ConsumerReference
        };
    }
}