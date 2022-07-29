using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgress.Api.AppStart;

public static class AddServiceRegistrationExtension
{
    public static void AddServiceRegistration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        //services.AddTransient<ICacheStorageService, CacheStorageService>();
        //services.AddTransient<IAccountLegalEntityPermissionService, AccountLegalEntityPermissionService>();

        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        //services.AddTransient<IRecruitApiClient<RecruitApiConfiguration>, RecruitApiClient>();
        //services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        //services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        //services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
    }
}