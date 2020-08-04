using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.SharedOuterApi.AppStart
{
    public static class SharedConfigurationBuilderExtension
    {
        public static IConfigurationRoot BuildSharedConfiguration(this IConfiguration configuration, IWebHostEnvironment env = default)
        {
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables();

#if DEBUG
            config.AddJsonFile("appsettings.json", true);
#endif

            if (!configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase) && !env.IsLocalAcceptanceTests())
            {
                config.AddAzureTableStorage(options =>
                    {
                        options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                        options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                        options.EnvironmentName = configuration["Environment"];
                        options.PreFixConfigurationKeys = false;
                    }
                );
            }
#if DEBUG
            if (!env.IsLocalAcceptanceTests())
            {
                config.AddJsonFile("appsettings.Development.json", true);
            }
#endif

            return config.Build();
        }

        public static bool IsLocalOrDev(this IConfiguration configuration)
        {
            return configuration["Environment"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) ||
                   configuration["Environment"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsLocalAcceptanceTests(this IWebHostEnvironment environment)
        {
            return environment?.IsEnvironment("LOCAL_ACCEPTANCE_TESTS") ?? false;
        }

    }
}