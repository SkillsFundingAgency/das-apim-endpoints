using SFA.DAS.Payments.EarningEvents.Messages.External.Commands;

namespace SFA.DAS.LearnerData.Events;

public class GrowthAndSkillsPaymentsRecalculatedEvent
{
    public CalculateGrowthAndSkillsPayments Command { get; set; }
}
