﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.TrainingTypesApi;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Approvals.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        
        services.Configure<FjaaApiConfiguration>(configuration.GetSection(nameof(FjaaApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FjaaApiConfiguration>>().Value);    
        
        services.Configure<ProviderRelationshipsApiConfiguration>(configuration.GetSection(nameof(ProviderRelationshipsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderRelationshipsApiConfiguration>>().Value);    
        
        services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
        services.Configure<AccountsConfiguration>(configuration.GetSection("AccountsApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AccountsConfiguration>>().Value);
        services.Configure<AssessorsApiConfiguration>(configuration.GetSection(nameof(AssessorsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AssessorsApiConfiguration>>().Value);
        services.Configure<ApprenticeCommitmentsApiConfiguration>(configuration.GetSection(nameof(ApprenticeCommitmentsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeCommitmentsApiConfiguration>>().Value);
        services.Configure<ApprenticeAccountsApiConfiguration>(configuration.GetSection(nameof(ApprenticeAccountsApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeAccountsApiConfiguration>>().Value);
        services.Configure<LevyTransferMatchingApiConfiguration>(configuration.GetSection(nameof(LevyTransferMatchingApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LevyTransferMatchingApiConfiguration>>().Value);
        services.Configure<ProviderCoursesApiConfiguration>(configuration.GetSection(nameof(ProviderCoursesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderCoursesApiConfiguration>>().Value);
        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);
        services.Configure<CommitmentsV2ApiConfiguration>(configuration.GetSection(nameof(CommitmentsV2ApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CommitmentsV2ApiConfiguration>>().Value);
        services.Configure<ProviderAccountApiConfiguration>(configuration.GetSection(nameof(ProviderAccountApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderAccountApiConfiguration>>().Value);
        services.Configure<ReservationApiConfiguration>(configuration.GetSection(nameof(ReservationApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ReservationApiConfiguration>>().Value);
        services.Configure<ProviderEventsConfiguration>(configuration.GetSection(nameof(ProviderEventsConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ProviderEventsConfiguration>>().Value);
        services.Configure<RoatpConfiguration>(configuration.GetSection(nameof(RoatpConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpConfiguration>>().Value);
        services.Configure<EmployerProfilesApiConfiguration>(configuration.GetSection(nameof(EmployerProfilesApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EmployerProfilesApiConfiguration>>().Value);
        services.Configure<TrainingProviderConfiguration>(configuration.GetSection("TrainingProviderApi"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingProviderConfiguration>>().Value);
        services.Configure<RoatpV2ApiConfiguration>(configuration.GetSection("ProviderCoursesApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpV2ApiConfiguration>>().Value);
        services.Configure<ApprenticeshipsApiConfiguration>(configuration.GetSection("ApprenticeshipsApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<ApprenticeshipsApiConfiguration>>().Value);
        services.Configure<CollectionCalendarApiConfiguration>(configuration.GetSection("CollectionCalendarApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<CollectionCalendarApiConfiguration>>().Value);
        services.Configure<FinanceApiConfiguration>(configuration.GetSection("FinanceApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<FinanceApiConfiguration>>().Value);
        services.Configure<LearnerDataInnerApiConfiguration>(configuration.GetSection("LearnerDataInnerApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LearnerDataInnerApiConfiguration>>().Value);
        services.Configure<TrainingTypesApiConfiguration>(configuration.GetSection("TrainingTypesApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<TrainingTypesApiConfiguration>>().Value);
    }
}