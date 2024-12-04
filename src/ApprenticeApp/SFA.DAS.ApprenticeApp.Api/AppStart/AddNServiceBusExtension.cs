using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.PushNotifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeApp.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddNServiceBusExtension
    {
        public static async Task<UpdateableServiceProvider> StartServiceBus
        (
            this UpdateableServiceProvider serviceProvider,
            IConfiguration configuration,
            string endpointName
        )
        {
            var config = configuration
            .GetSection("NServiceBusConfiguration")
            .Get<NServiceBusConfiguration>();

            var endpointConfiguration = new EndpointConfiguration(endpointName)
                .UseErrorQueue($"{endpointName}-errors")
                .UseInstallers()
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer();

            if (config != null)
            {
                if (!string.IsNullOrEmpty(config.NServiceBusLicense))
                {
                    endpointConfiguration.UseLicense(config.NServiceBusLicense);
                }
                endpointConfiguration.SendOnly();

                if (config.NServiceBusConnectionString.Equals("UseLearningEndpoint=true", StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(config.NServiceBusConnectionString))
                {
                    endpointConfiguration.UseLearningTransport(s => s.AddRouting(endpointName));
                }
                else
                {
                    var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
                    transport.ConnectionString(config.NServiceBusConnectionString);

                    var routing = transport.Routing();
                    routing.AddRouting(endpointName);
                }

                var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

                serviceProvider.AddSingleton(p => endpoint)
                    .AddSingleton<IMessageSession>(p => p.GetService<IEndpointInstance>())
                    .AddHostedService<NServiceBusHostedService>();

                return serviceProvider;
            }
            else
            {
                return null;
            }
        }

        public static void AddRouting(this RoutingSettings routingSettings, string endpointName)
        {
            routingSettings.RouteToEndpoint(typeof(AddWebPushSubscriptionCommand), endpointName);
            routingSettings.RouteToEndpoint(typeof(RemoveWebPushSubscriptionCommand), endpointName);
        }
    }
}