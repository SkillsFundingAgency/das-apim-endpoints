using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.LearnerData.Api.AppStart;

public static class AddApiServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<ILearningApiClient<LearningApiConfiguration>, LearningApiClient>();
        services.AddTransient<IEarningsApiClient<EarningsApiConfiguration>, EarningsApiClient>();
        services.AddTransient<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>, CollectionCalendarApiClient>();
        services.AddTransient<ILearningSupportService, LearningSupportService>();
        services.AddTransient<IBreaksInLearningService, BreaksInLearningService>();
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<IUpdateLearningPutRequestBuilder, UpdateLearningPutRequestBuilder>();
        services.AddTransient<IUpdateEarningsOnProgrammeRequestBuilder, UpdateEarningsOnProgrammeRequestBuilder>();
        services.AddTransient<IUpdateEarningsEnglishAndMathsRequestBuilder, UpdateEarningsEnglishAndMathsRequestBuilder>();
        services.AddTransient<IUpdateEarningsLearningSupportRequestBuilder, UpdateEarningsLearningSupportRequestBuilder>();
        services.AddTransient<ICostsService, CostsService>();
        services.AddTransient<ILearnerDataCacheService, LearnerDataCacheService>();
        services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<IFjaaApiClient<FjaaApiConfiguration>, FjaaApiClient>();
        services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
        services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();
        services.AddTransient<IGetProviderRelationshipService, GetProviderRelationshipService>();
    }
}
