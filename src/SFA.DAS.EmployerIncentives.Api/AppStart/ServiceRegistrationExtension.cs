using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Services;

namespace SFA.DAS.EmployerIncentives.Api.AppStart
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection AddDasHttpClients(this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env)
        {
            services.AddTransient(typeof(ManagedIdentityApiHandler));
            services.AddTransient<IRestApiClient, RestApiClient>();

            var httpBuilder = services.AddHttpClient("EmployerIncentivesInnerApi", c =>
                {
                    var apiConfig = GetConfigSection(configuration, EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration);
                    c.BaseAddress = new Uri(apiConfig.Url);
                })
                .AddTypedClient<IEmployerIncentivesPassThroughService, EmployerIncentivesPassThroughService>();

            if (!env.IsDevelopment())
            {
                httpBuilder.AddHttpMessageHandler(_ =>
                {
                    var apiConfig = GetConfigSection(configuration, EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration);
                    return new ManagedIdentityApiHandler(apiConfig);
                });
            }

            return services;
        }

        private static AzureManagedIdentityApiConfiguration GetConfigSection(IConfiguration configuration, string section)
        {
            return configuration.GetSection(section).Get<AzureManagedIdentityApiConfiguration>();
        }
    }
}