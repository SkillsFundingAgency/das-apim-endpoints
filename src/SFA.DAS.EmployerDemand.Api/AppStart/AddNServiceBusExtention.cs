using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NServiceBus;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.Configuration.NLog;
using SFA.DAS.NServiceBus.Hosting;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.EmployerDemand.Api.AppStart
{
    public static class AddNServiceBusExtention
    {
        private const string EndpointName = "SFA.DAS.EmployerDemand";

        public static IServiceCollection AddNServiceBus(this IServiceCollection services)
        {
            return services
                .AddSingleton(p =>
                {
                    var sp = services.BuildServiceProvider();
                    var configuration = sp.GetService<IOptions<NServiceBusConfiguration>>().Value;

                    var hostingEnvironment = p.GetService<IWebHostEnvironment>();

                    var endpointConfiguration = new EndpointConfiguration(EndpointName)
                        .UseErrorQueue($"{EndpointName}-errors")
                        
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
                .AddSingleton<IMessageSession>(p => p.GetService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();
        }
    }
}
