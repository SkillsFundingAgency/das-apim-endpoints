using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;
using SFA.DAS.VacanciesManage.Services;
//using RecruitApiConfiguration = SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration;

namespace SFA.DAS.VacanciesManage.Api.AppStart;

public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

        services.AddTransient<ICacheStorageService, CacheStorageService>();
        services.AddTransient<IAccountLegalEntityPermissionService, AccountLegalEntityPermissionService>();
        services.AddTransient<ITrainingProviderService, TrainingProviderService>();

        services.AddTransient<IBankHolidaysService, BankHolidaysService>();
        services.AddTransient<IBankHolidayProvider, BankHolidayProvider>();
        services.AddTransient<ISlaService, SlaService>();

        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<IRecruitApiClient<RecruitApiConfiguration>, RecruitApiClient>();

        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
    }
}