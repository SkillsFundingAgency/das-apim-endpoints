﻿using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;

namespace SFA.DAS.ReferenceDataJobs.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AuthenticationServiceExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfigurationRoot configuration)
    {
        if (!configuration.IsLocalOrDev())
        {
            var azureAdConfiguration = configuration
                .GetSection("AzureAd")
                .Get<AzureActiveDirectoryConfiguration>();
            var policies = new Dictionary<string, string>
            {
                {"default", "APIM"}
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }
        return services;
    }
}