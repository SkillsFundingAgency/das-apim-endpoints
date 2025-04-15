﻿using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.ApprenticeshipsManage.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>()!.Value);

        services.Configure<ApprenticeshipsApiConfiguration>(configuration.GetSection(nameof(ApprenticeshipsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeshipsApiConfiguration>>()!.Value);
        services.Configure<CollectionCalendarApiConfiguration>(configuration.GetSection(nameof(CollectionCalendarApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CollectionCalendarApiConfiguration>>()!.Value);

        var azureAdConfiguration = configuration
                .GetSection("CollectionCalendarApiConfiguration")
                .Get<CollectionCalendarApiConfiguration>();
    }
}
