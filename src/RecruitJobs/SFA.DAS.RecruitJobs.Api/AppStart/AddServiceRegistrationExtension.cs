using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.RecruitJobs.Ai;
using SFA.DAS.RecruitJobs.Api.Models.Mappers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.RecruitJobs.Handlers;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Infrastructure.Services;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Services;
using SFA.DAS.SharedOuterApi.Recruit.Services;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;
using BankHolidaysService = SFA.DAS.SharedOuterApi.Recruit.Services.BankHolidaysService;
using IBankHolidaysService = SFA.DAS.SharedOuterApi.Recruit.Services.IBankHolidaysService;

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
        services.AddTransient<ILocationApiClient<LocationApiConfiguration>, LocationApiClient>();
        services.AddTransient<IBusinessMetricsApiClient<BusinessMetricsConfiguration>, BusinessMetricsApiClient>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<ICoursesApiClient<CoursesApiConfiguration>, CourseApiClient>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<IRecruitAiService, RecruitAiService>();
        services.AddTransient<VacancyMapper>();
        services.AddTransient<ITransferProviderVacancyToLegalEntityHandler, TransferProviderVacancyToLegalEntityHandler>();
        services.AddTransient<ITransferProviderVacancyToQaReviewHandler, TransferProviderVacancyToQaReviewHandler>();
        services.AddKeyedTransient<IBankHolidaysService, BankHolidaysService>("vanilla");
        services.AddTransient<IBankHolidaysService, CachingBankHolidaysService>();
        services.AddTransient<IVacancySlaDeadlineService, VacancySlaDeadlineService>();
        services.AddTransient<IVacancyComparerService, VacancyComparerService>();
        services.AddTransient<IVacancyReviewService, VacancyReviewService>();
        services.AddTransient<ILocationLookupService, LocationLookupService>();
        services.AddTransient<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>, SFA.DAS.Recruit.Contracts.Client.RecruitApiClient>();
    }
}