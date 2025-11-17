using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.Earnings.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient<ILearningApiClient<LearningApiConfiguration>, LearningApiClient>();
        services.AddTransient<IEarningsApiClient<EarningsApiConfiguration>, EarningsApiClient>();
        services.AddTransient<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>, CollectionCalendarApiClient>();
    }
}