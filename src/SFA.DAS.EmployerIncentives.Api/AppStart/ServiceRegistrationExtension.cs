//using System;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using SFA.DAS.EmployerIncentives.Configuration;
//using SFA.DAS.EmployerIncentives.Interfaces;
//using SFA.DAS.EmployerIncentives.Services;
//using SFA.DAS.SharedOuterApi.Infrastructure;
//using SFA.DAS.SharedOuterApi.Interfaces;

//namespace SFA.DAS.EmployerIncentives.Api.AppStart
//{
//    public static class ServiceRegistrationExtension
//    {
//        public static IServiceCollection AddDasHttpClientsAndAssociatedServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
//        {
//            services.AddTransient<IRestApiClient, RestApiClient>()
//                .AddTypedHttpClient<IPassThroughApiClient, PassThroughApiClient>(configuration, env, EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration, "EmployerIncentivesPassThroughClient")
//                .AddTypedHttpClient<IEmployerIncentivesService, EmployerIncentivesService>(configuration, env, EmployerIncentivesConfigurationKeys.EmployerIncentivesInnerApiConfiguration, "EmployerIncentivesClient")
//                .AddTypedHttpClient<ICommitmentsV2Service, CommitmentsV2Service>(configuration, env, EmployerIncentivesConfigurationKeys.CommitmentsV2InnerApiConfiguration, "CommitmentsV2Client");

//            return services;
//        }

//        public static IServiceCollection AddTypedHttpClient<TService, TImplementation>(this IServiceCollection services,
//            IConfiguration configuration, IWebHostEnvironment env, string configSection, string apiName) where TService : class where TImplementation : class, TService
//        {
//            services.AddHttpClient(apiName, c =>
//                {
//                    var apiConfig = GetConfigSection(configuration, configSection);
//                    c.BaseAddress = new Uri(apiConfig.Url);
//                })
//                .AddTypedClient<TService, TImplementation>()
//                .AddLoggingApiHandler<TImplementation>()
//                .AddManagedIdentityApiHandler(configuration, env, configSection);

//            return services;
//        }

//        public static IHttpClientBuilder AddManagedIdentityApiHandler(this IHttpClientBuilder httpBuilder, IConfiguration configuration, IWebHostEnvironment env, string configSection)
//        {
//            if (!env.IsDevelopment() && !env.IsEnvironment("LOCAL"))
//            {
//                httpBuilder.AddHttpMessageHandler(_ =>
//                {
//                    var apiConfig = GetConfigSection(configuration, configSection);
//                    return new ManagedIdentityApiHandler(apiConfig.Identifier);
//                });
//            }

//            return httpBuilder;
//        }

//        public static IHttpClientBuilder AddLoggingApiHandler<T>(this IHttpClientBuilder httpBuilder)
//        {
//            httpBuilder.AddHttpMessageHandler(sp =>
//            {
//                var logger = sp.GetService<ILogger<T>>();   
//                return new LoggingApiHandler<T>(logger);
//            });

//            return httpBuilder;
//        }

//        private static AzureManagedIdentityApiConfiguration GetConfigSection(IConfiguration configuration, string section)
//        {
//            return configuration.GetSection(section).Get<AzureManagedIdentityApiConfiguration>();
//        }
//    }
//}