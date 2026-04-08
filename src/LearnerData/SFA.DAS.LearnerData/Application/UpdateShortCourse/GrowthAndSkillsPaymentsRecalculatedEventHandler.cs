using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.LearnerData.Events;

namespace SFA.DAS.LearnerData.Application.UpdateShortCourse;

public class GrowthAndSkillsPaymentsRecalculatedEventHandler(
    ILogger<GrowthAndSkillsPaymentsRecalculatedEventHandler> logger,
    PaymentsConfiguration paymentsConfiguration)
    : IHandleMessages<GrowthAndSkillsPaymentsRecalculatedEvent>
{
    public async Task Handle(GrowthAndSkillsPaymentsRecalculatedEvent message, IMessageHandlerContext context)
    {
        logger.LogInformation("Sending CalculateGrowthAndSkillsPayments command for LearningKey: {LearningKey}", message.Command.Training?.LearningKey);

        var options = new SendOptions();
        options.DoNotEnforceBestPractices();
        options.SetDestination(paymentsConfiguration.PaymentsEndpoint);
        await context.Send(message.Command, options);

        logger.LogInformation("CalculateGrowthAndSkillsPayments command sent for LearningKey: {LearningKey}", message.Command.Training?.LearningKey);
    }
}
