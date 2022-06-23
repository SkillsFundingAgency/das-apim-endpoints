using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Approvals.Api.AppStart
{
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<FjaaApiConfiguration>(configuration.GetSection(nameof(FjaaApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<FjaaApiConfiguration>>().Value);
            services.Configure<CoursesApiConfiguration>(configuration.GetSection(nameof(CoursesApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CoursesApiConfiguration>>().Value);
            services.Configure<CourseDeliveryApiConfiguration>(configuration.GetSection(nameof(CourseDeliveryApiConfiguration)));
            services.AddSingleton(cfg => cfg.GetService<IOptions<CourseDeliveryApiConfiguration>>().Value);
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

        }
    }
}