using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Services;

namespace SFA.DAS.EmployerIncentives.Api.AppStart
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection AddDasHttpClientsAndAssociatedServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddTransient(typeof(ManagedIdentityApiHandler))
                .AddTransient<IRestApiClient, RestApiClient>()
                .AddEmployerIncentivesHttpClient(configuration, env)
                .AddCommitmentsV2HttpClient(configuration, env);

            return services;
        }

        public static IServiceCollection AddEmployerIncentivesHttpClient(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment env)
        {
            var configSection = EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration;
            services.AddHttpClient("EmployerIncentivesInnerApi", c =>
                {
                    var apiConfig = GetConfigSection(configuration, configSection);
                    c.BaseAddress = new Uri(apiConfig.Url);
                })
                .AddTypedClient<IEmployerIncentivesPassThroughService, EmployerIncentivesPassThroughService>()
                .AddManagedIdentityApiHandler(configuration, env, configSection);

            return services;
        }

        public static IServiceCollection AddCommitmentsV2HttpClient(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment env)
        {
            var configSection = EmployerIncentivesConfigurationKeys.CommitmentsV2InnerApiConfiguration;
            services.AddHttpClient("CommitmentsV2InnerApi", c =>
                {
                    var apiConfig = GetConfigSection(configuration, configSection);
                    c.BaseAddress = new Uri(apiConfig.Url);
                })
                .AddTypedClient<ICommitmentsV2Service, CommitmentsV2Service>()
                .AddManagedIdentityApiHandler(configuration, env, configSection);

            return services;
        }

        public static IHttpClientBuilder AddManagedIdentityApiHandler(this IHttpClientBuilder httpBuilder, IConfiguration configuration, IWebHostEnvironment env, string configSection)
        {
            if (!env.IsDevelopment())
            {
                httpBuilder.AddHttpMessageHandler(_ =>
                {
                    var apiConfig = GetConfigSection(configuration, configSection);
                    return new ManagedIdentityApiHandler(apiConfig);
                });
            }

            return httpBuilder;
        }

        private static AzureManagedIdentityApiConfiguration GetConfigSection(IConfiguration configuration, string section)
        {
            return configuration.GetSection(section).Get<AzureManagedIdentityApiConfiguration>();
        }
    }
}