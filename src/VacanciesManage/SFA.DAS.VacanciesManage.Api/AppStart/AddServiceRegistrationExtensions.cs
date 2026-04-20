using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.VacanciesManage.Services;
using CacheStorageService = SFA.DAS.Apim.Shared.Infrastructure.Services.CacheStorageService;
using ICacheStorageService = SFA.DAS.Apim.Shared.Interfaces.ICacheStorageService;
using RecruitApiConfiguration = SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration;

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

        services.AddTransient(typeof(IInternalApiClient<>), typeof(SharedOuterApi.Infrastructure.InternalApiClient<>));
        services.AddTransient(typeof(Apim.Shared.Interfaces.IInternalApiClient<>), typeof(Apim.Shared.Infrastructure.InternalApiClient<>));
        services.AddTransient<Recruit.Contracts.Client.IRecruitApiClient<RecruitApiConfiguration>, Recruit.Contracts.Client.RecruitApiClient>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
    }
}