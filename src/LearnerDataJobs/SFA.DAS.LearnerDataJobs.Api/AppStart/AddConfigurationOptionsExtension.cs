﻿using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.LearnerDataJobs.InnerApi;

namespace SFA.DAS.LearnerDataJobs.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        services.Configure<LearnerDataInnerApiConfiguration>(configuration.GetSection("LearnerDataInnerApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LearnerDataInnerApiConfiguration>>().Value);
    }

}
