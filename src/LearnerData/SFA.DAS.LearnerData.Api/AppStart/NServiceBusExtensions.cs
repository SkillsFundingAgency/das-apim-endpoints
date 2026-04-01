using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using System.Text.RegularExpressions;

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
                Regex.IsMatch(t.Name, "Event(V\\d+)?$")
                || typeof(Event).IsAssignableFrom(t)
                || t.FullName == "SFA.DAS.Payments.EarningEvents.Messages.External.Commands.CalculateGrowthAndSkillsPayments"
            );

        return config;
    }
}
