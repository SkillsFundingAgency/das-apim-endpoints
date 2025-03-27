﻿using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.AppStart;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.LearnerDataJobs.Api.AppStart;

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
        }

        return services;
    }
}