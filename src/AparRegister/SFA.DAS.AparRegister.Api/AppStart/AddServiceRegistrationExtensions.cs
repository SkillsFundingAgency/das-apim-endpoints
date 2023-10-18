using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.AparRegister.Api.AppStart
{
    public static class AddServiceRegistrationExtensions
    {
        public static void AddServiceRegistration(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GetProvidersQuery).Assembly);
            
            services.AddHttpClient();
            services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
            
            services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
            services.AddTransient<IRoatpServiceApiClient<RoatpConfiguration>, RoatpServiceApiClient>();
            services.AddTransient<ICacheStorageService, CacheStorageService>();
        }
    }
}