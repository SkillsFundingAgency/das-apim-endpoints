using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.ApprenticeCommitments.Api.Controllers;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.ApprenticeCommitments.Api.AppStart
{
    public static class AddServiceRegistrationExtension
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();

            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<CourseApiClient>();
            services.AddTransient<ApprenticeCommitmentsService>();
            services.AddTransient<CommitmentsV2Service>();
            services.AddTransient<ITrainingProviderApiClient<TrainingProviderConfiguration>, TrainingProviderApiClient>();
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
            
            services.AddTransient<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>, ApprenticeAccountsApiClient>();
        }
    }
}