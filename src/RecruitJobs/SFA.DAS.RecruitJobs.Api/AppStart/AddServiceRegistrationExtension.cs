using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Ai.Clients;
using SFA.DAS.RecruitJobs.Ai.Services;
using SFA.DAS.RecruitJobs.Api.Models.Mappers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.RecruitJobs.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<ICacheStorageService, CacheStorageService>();
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IRecruitApiClient<RecruitApiConfiguration>, RecruitApiClient>();
        services.AddTransient<IRecruitAiApiClient<RecruitAiApiConfiguration>, RecruitAiApiClient>();
        services.AddTransient<IBusinessMetricsApiClient<BusinessMetricsConfiguration>, BusinessMetricsApiClient>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<IRandomNumberGenerator, RandomNumberGenerator>();
        services.AddTransient<IAiReviewResultChecker, AiReviewResultChecker>();
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<IAzureAiClient, AzureAiClient>();
        services.AddTransient<IRecruitArtificialIntelligenceService, RecruitArtificialIntelligenceService>();
        services.AddTransient<VacancyMapper>();
    }
}