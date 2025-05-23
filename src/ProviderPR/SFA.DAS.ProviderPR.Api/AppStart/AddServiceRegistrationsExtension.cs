﻿using System.Diagnostics.CodeAnalysis;
using RestEase.HttpClientFactory;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationship;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Provider.DfeSignIn.Auth.Application.Queries.ProviderAccounts;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ProviderPR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationsExtension
{
    public static IServiceCollection AddServiceRegistration(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetRoatpV2ProviderQuery).Assembly));
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetRelationshipQuery).Assembly));
        services.AddTransient<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>, ProviderRelationshipsApiClient>();
        services.AddTransient<IPensionRegulatorApiClient<PensionRegulatorApiConfiguration>, PensionRegulatorApiClient>();
        services.AddHttpClient();

        AddProviderRelationshipsApiClient(services, configuration);

        RegisterExternalApiClients(services);

        return services;
    }

    private static void AddProviderRelationshipsApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiConfig = GetApiConfiguration(configuration, "ProviderRelationshipsApiConfiguration");

        services.AddRestEaseClient<IProviderRelationshipsApiRestClient>(apiConfig.Url)
            .AddHttpMessageHandler(() => new InnerApiAuthenticationHeaderHandler(new AzureClientCredentialHelper(configuration), apiConfig.Identifier));
    }

    private static InnerApiConfiguration GetApiConfiguration(IConfiguration configuration, string configurationName)
        => configuration.GetSection(configurationName).Get<InnerApiConfiguration>()!;

    private static void RegisterExternalApiClients(IServiceCollection services)
    {
        services.AddSingleton<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));

        services.AddTransient<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>, RoatpCourseManagementApiClient>();
        services.AddTransient<IRoatpV2TrainingProviderService, RoatpV2TrainingProviderService>();
        services.AddTransient<IAccountsApiClient<AccountsConfiguration>, AccountsApiClient>();
    }
}
