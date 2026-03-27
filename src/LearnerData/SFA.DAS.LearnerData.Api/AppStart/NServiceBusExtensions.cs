using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;

namespace SFA.DAS.LearnerData.Api.AppStart;

public static class NServiceBusExtensions
{
    public static EndpointConfiguration UseExtendedMessageConventions(this EndpointConfiguration config)
    {
        // Call shared conventions first
        config.UseMessageConventions();

        // Then extend
        config.Conventions()
            .DefiningEventsAs(t =>
                t.FullName == "SFA.DAS.Payments.EarningEvents.Messages.External.Commands.CalculateGrowthAndSkillsPayments"
                || t.Namespace != null && t.Namespace.Contains(".Commands")
            );

        return config;
    }
}
