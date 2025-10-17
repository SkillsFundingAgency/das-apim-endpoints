using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.EmployerFeedback.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.SharedOuterApi.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerFeedback.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
            services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsInnerApi"));
            services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection(nameof(CommitmentsV2ApiConfiguration)));
            services.Configure<EmployerFeedbackApiConfiguration>(configuration.GetSection(nameof(EmployerFeedbackApiConfiguration)));
            services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
            services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection(nameof(RoatpV2ApiConfiguration)));
            services.Configure<EmployerFeedbackConfiguration>(configuration.GetSection(nameof(EmployerFeedbackConfiguration)));
            services.Configure<NServiceBusConfiguration>(configuration.GetSection(nameof(NServiceBusConfiguration)));

            services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerFeedbackApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerFeedbackConfiguration>>().Value);
            services.AddSingleton(cfg => cfg.GetService<IOptions<NServiceBusConfiguration>>().Value);

            var dasEncodingConfig = new EncodingConfig { Encodings = [] };
            configuration.GetSection(nameof(dasEncodingConfig.Encodings)).Bind(dasEncodingConfig.Encodings);
            services.AddSingleton(dasEncodingConfig);

            //var encodingConfigJson = configuration.GetSection("SFA.DAS.Encoding").Value;
            //var encodingConfig = JsonConvert.DeserializeObject<EncodingConfig>(encodingConfigJson);
            //services.AddSingleton(encodingConfig);
        }
    }
}