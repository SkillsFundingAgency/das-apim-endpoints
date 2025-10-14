using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Events;

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

        return new LearnerDataEvent
        {
            ULN = long.Parse(command.Request.Learner.Uln),
            UKPRN = command.Ukprn,
            FirstName = command.Request.Learner.Firstname,
            LastName = command.Request.Learner.Lastname,
            Email = command.Request.Learner.Email,
            DoB = command.Request.Learner.Dob!.Value,
            StartDate = command.Request.Delivery.OnProgramme.StartDate!.Value,
            PlannedEndDate = command.Request.Delivery.OnProgramme.ExpectedEndDate!.Value,
            EpaoPrice = command.Request.Delivery.OnProgramme.Costs.Single().EpaoPrice ?? 0,
            TrainingPrice = command.Request.Delivery.OnProgramme.Costs.Single().TrainingPrice ?? 0,
            AgreementId = command.Request.Delivery.OnProgramme.AgreementId,
            IsFlexiJob = command.Request.Delivery.OnProgramme.IsFlexiJob!.Value,
            StandardCode = command.Request.Delivery.OnProgramme.StandardCode!.Value,
            CorrelationId = command.CorrelationId,
            ReceivedDate = command.ReceivedOn,
            ConsumerReference = command.Request.ConsumerReference
        };
    }
}