using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Api.Models.Mappers;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Services;
using SFA.DAS.SharedOuterApi.Types.Services;

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
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<IRecruitAiService, RecruitAiService>();
        services.AddTransient<VacancyMapper>();
    }
}