using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration.NLog;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.SharedOuterApi.Configuration;
using System;

namespace SFA.DAS.LevyTransferMatching.Api.AppStart
{
    public static class AddNServiceBusExtension
    {
        private const string EndpointName = "SFA.DAS.LevyTransferMatching.Api";

        public static IServiceCollection AddNServiceBus(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var logger = sp.GetRequiredService<ILogger<PledgeController>>();

            try
            {
                return services
                    .AddSingleton(p =>
                    {
                        logger.LogInformation("Getting config");
                        var configuration = sp.GetService<IOptions<NServiceBusConfiguration>>().Value;
                        logger.LogInformation("Successfully loaded config");

                        var hostingEnvironment = p.GetService<IHostingEnvironment>();
                        logger.LogInformation("Got hosting environment");

                        var endpointConfiguration = new EndpointConfiguration(EndpointName)
                            .UseErrorQueue($"{EndpointName}-errors")
                            .UseInstallers()
                            .UseMessageConventions()
                            .UseNewtonsoftJsonSerializer()
                            .UseNLogFactory();

                        logger.LogInformation("Got endpoint configuration");

                        if (!string.IsNullOrEmpty(configuration.NServiceBusLicense))
                        {
                            endpointConfiguration.UseLicense(configuration.NServiceBusLicense);
                        }

                        logger.LogInformation("License configured");

                        endpointConfiguration.SendOnly();

                        if (hostingEnvironment.IsDevelopment())
                        {
                            endpointConfiguration.UseLearningTransport(s => s.AddRouting());
                        }
                        else
                        {
                            endpointConfiguration.UseAzureServiceBusTransport(configuration.SharedServiceBusEndpointUrl, s => s.AddRouting());
                        }

                        logger.LogInformation("Configured azure service bus transport");

                        var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

                        logger.LogInformation("Started endpoint");

                        return endpoint;
                    })
                    .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
                    .AddHostedService<NServiceBusHostedService>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
    }

    public static class RoutingSettingsExtensions
    {
        private const string NotificationsMessageHandler = "SFA.DAS.Notifications.MessageHandlers";

        public static void AddRouting(this RoutingSettings routingSettings)
        {
            routingSettings.RouteToEndpoint(typeof(SendEmailCommand), NotificationsMessageHandler);
        }
    }
}
