using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;

namespace SFA.DAS.LearnerData.Api.AppStart;

public static class NServiceBusExtensions
{
    public static EndpointConfiguration UseExtendedMessageConventions(this EndpointConfiguration config)
    {
        config.UseMessageConventions();

        config.Conventions()
            .DefiningCommandsAs(t =>
                t.FullName == "SFA.DAS.Payments.EarningEvents.Messages.External.Commands.CalculateGrowthAndSkillsPayments"
            );

        return config;
    }
}
