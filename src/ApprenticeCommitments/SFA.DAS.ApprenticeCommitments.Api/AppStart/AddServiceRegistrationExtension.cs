using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ApprenticeCommitments.Api.Controllers;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Net.Http;

namespace SFA.DAS.ApprenticeCommitments.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient<ApprenticeLoginClient>();
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IInternalApiClient<ApprenticeLoginConfiguration>, ApprenticeLoginClient>();
            services.AddTransient<CourseApiClient>();
            services.AddTransient<ApprenticeCommitmentsService>();
            services.AddTransient<ApprenticeLoginService>();
            services.AddTransient<CommitmentsV2Service>();
            services.AddTransient<TrainingProviderService>();
            services.AddTransient<CoursesService>();
            services.AddTransient<ApimClient>();
            services.AddTransient<ResponseReturningApiClient>();
            services.AddTransient(s =>
                new TemporaryAccountsResponseReturningApiClient(
                    new ApimClient(
                        s.GetRequiredService<IHttpClientFactory>(),
                        s.GetRequiredService<ApprenticeAccountsConfiguration>(),
                        s.GetRequiredService<IWebHostEnvironment>(),
                        s.GetRequiredService<IAzureClientCredentialHelper>())));
        }
    }
}