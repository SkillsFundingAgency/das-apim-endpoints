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
            return services
                .AddSingleton(p =>
                {
                    var sp = services.BuildServiceProvider();
                    var configuration = sp.GetService<IOptions<NServiceBusConfiguration>>().Value;

                    var hostingEnvironment = p.GetService<IHostingEnvironment>();

                    var endpointConfiguration = new EndpointConfiguration(EndpointName)
                        .UseErrorQueue($"{EndpointName}-errors")
                        .UseInstallers()
                        .UseMessageConventions()
                        .UseNewtonsoftJsonSerializer()
                        .UseNLogFactory();

                    if (!string.IsNullOrEmpty(configuration.NServiceBusLicense))
                    {
                        endpointConfiguration.UseLicense(configuration.NServiceBusLicense);
                    }

                    endpointConfiguration.SendOnly();

                    if (hostingEnvironment.IsDevelopment())
                    {
                        endpointConfiguration.UseLearningTransport(s => s.AddRouting());
                    }
                    else
                    {
                        endpointConfiguration.UseAzureServiceBusTransport(configuration.NServiceBusConnectionString, s => s.AddRouting());
                    }

                    var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

                    return endpoint;
                })
                .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();
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
