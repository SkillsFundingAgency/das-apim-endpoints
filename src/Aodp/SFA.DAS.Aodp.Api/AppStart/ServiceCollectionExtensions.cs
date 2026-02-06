using SFA.DAS.Aodp.Services;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Authentication;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.Api.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IAodpApiClient<AodpApiConfiguration>, AodpApiClient>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddScoped<IEmailService, EmailService>();
           
            #region DFE Sign In API
            services.AddSingleton<IDfeJwtProvider, DfeJwtProvider>();
            services.AddTransient<IDfeUsersApiClient<DfeSignInApiConfiguration>, DfeUsersApiClient>();
            services.AddTransient<IDfeUsersService, DfeUsersService>();
            #endregion

            return services;
        }
    }
}
