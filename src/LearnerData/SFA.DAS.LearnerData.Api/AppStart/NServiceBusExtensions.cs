using Azure.Identity;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using System.Net;
using System.Security.Cryptography;

namespace SFA.DAS.LearnerData.Api.AppStart;

public static class NServiceBusExtensions
{
    public const string EndpointName = "SFA.DAS.LearnerData.OuterApi";

    public static void AddNServiceBus(this IHostBuilder hostBuilder, IConfiguration configuration)
    {
        var nsbConnection = configuration["NServiceBusConfiguration:NServiceBusConnectionString"];
        if (string.IsNullOrEmpty(nsbConnection)) return;

        hostBuilder.UseNServiceBus(_ =>
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.UseExtendedMessageConventions();
            endpointConfiguration.UseNewtonsoftJsonSerializer();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo($"{EndpointName}-error");

            var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
            transport.SubscriptionRuleNamingConvention(ShortenRuleName);
            var fullyQualifiedNamespace = new Uri(
                nsbConnection.Split(';')
                    .First(p => p.StartsWith("Endpoint=", StringComparison.OrdinalIgnoreCase))
                    .Substring("Endpoint=".Length)
            ).Host;
            transport.ConnectionString(fullyQualifiedNamespace);
            transport.CustomTokenCredential(new DefaultAzureCredential());

            var decodedLicence = WebUtility.HtmlDecode(configuration["NServiceBusConfiguration:NServiceBusLicense"]);
            if (!string.IsNullOrWhiteSpace(decodedLicence)) endpointConfiguration.License(decodedLicence);

            return endpointConfiguration;
        });
    }

    public static EndpointConfiguration UseExtendedMessageConventions(this EndpointConfiguration config)
    {
        config.UseMessageConventions();

        config.Conventions()
            .DefiningCommandsAs(t =>
                t.FullName == "SFA.DAS.Payments.EarningEvents.Messages.External.Commands.CalculateGrowthAndSkillsPayments"
            );

        // SFA.DAS.NServiceBus contains client outbox handlers that require IClientOutboxStorage (SQL-backed).
        // We don't use the client outbox, so exclude it from handler scanning to avoid DI validation failures.
        config.AssemblyScanner().ExcludeAssemblies("SFA.DAS.NServiceBus.dll");

        return config;
    }

    public static string ShortenRuleName(Type type)
    {
        const int maxLength = 50;
        var ruleName = type.FullName!;
        if (ruleName.Length <= maxLength)
            return ruleName;

        var hash = MD5.HashData(System.Text.Encoding.Default.GetBytes(ruleName));
        return new Guid(hash).ToString();
    }
}
