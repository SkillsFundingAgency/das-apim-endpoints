﻿using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.LearnerData.InnerApi;

namespace SFA.DAS.LearnerData.Api.AppStart;
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetRequiredService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        services.Configure<LearnerDataInnerApiConfiguration>(configuration.GetSection(nameof(LearnerDataInnerApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LearnerDataInnerApiConfiguration>>().Value);
    }
}