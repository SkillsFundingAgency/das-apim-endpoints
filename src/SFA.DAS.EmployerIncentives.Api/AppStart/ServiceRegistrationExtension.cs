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
            services.AddTransient(typeof(ManagedIdentityApiHandler<>));
            services.AddTransient<IRestApiClient, RestApiClient>();

            var httpBuilder = services.AddHttpClient("EmployerIncentivesInnerApi", c =>
                {
                    var apiConfig = configuration
                        .GetSection(EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration)
                        .Get<EmployerIncentivesInnerApiConfiguration>();
                    c.BaseAddress = new Uri(apiConfig.Url);
                })
                .AddTypedClient<IEmployerIncentivesPassThroughService, EmployerIncentivesPassThroughService>();

            if (!env.IsDevelopment())
            {
                httpBuilder.AddHttpMessageHandler(_ =>
                {
                    var apiConfig = configuration
                        .GetSection(EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration)
                        .Get<EmployerIncentivesInnerApiConfiguration>();

                    return new ManagedIdentityApiHandler<EmployerIncentivesInnerApiConfiguration>(apiConfig);
                });
            }

            return services;
        }
    }
}