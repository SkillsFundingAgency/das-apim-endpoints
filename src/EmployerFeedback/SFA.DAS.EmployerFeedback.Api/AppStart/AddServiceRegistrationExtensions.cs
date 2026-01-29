using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EmployerFeedback.Api.TaskQueue;
using SFA.DAS.Encoding;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerFeedback.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
            services.AddTransient<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>, CommitmentsV2ApiClient>();
            services.AddTransient<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>, EmployerFeedbackApiClient>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddTransient<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>, EmployerProfilesApiClient>();
            services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
            services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();

            services.AddTransient<IEmployerAccountsService, EmployerAccountsService>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IEncodingService, EncodingService>();
        }
    }
}