﻿using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.FindApprenticeshipJobs.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    { 
        services.AddOptions();
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.Configure<RecruitApiConfiguration>(configuration.GetSection(nameof(RecruitApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiConfiguration>>().Value);
        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
        services.Configure<FindApprenticeshipJobsConfiguration>(configuration.GetSection(nameof(FindApprenticeshipJobsConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipJobsConfiguration>>().Value);
        services.Configure<LocationApiConfiguration>(configuration.GetSection(nameof(LocationApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LocationApiConfiguration>>().Value);
        services.Configure<CandidateApiConfiguration>(configuration.GetSection(nameof(CandidateApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CandidateApiConfiguration>>().Value);
        services.Configure<FindApprenticeshipApiConfiguration>(configuration.GetSection(nameof(FindApprenticeshipApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FindApprenticeshipApiConfiguration>>().Value);
        services.AddSingleton(new NhsJobsConfiguration());
        services.Configure<RecruitApiV2Configuration>(configuration.GetSection("RecruitAltApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RecruitApiV2Configuration>>().Value);
    }
}
