using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Api.Common.Interfaces;
using SFA.DAS.EarlyConnect.Api.Extensions;
using SFA.DAS.EarlyConnect.Configuration.FeatureToggle;
using SFA.DAS.EarlyConnect.Services;
using SFA.DAS.EarlyConnect.Services.Configuration;
using SFA.DAS.EarlyConnect.Services.Interfaces;
using SFA.DAS.EarlyConnect.Services.LepsApiClients;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EarlyConnect.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationExtensions
{
    public static void AddServiceRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        var credential = new DefaultAzureCredential();
        var certificateClient = new CertificateClient(new Uri(configuration["LepsLaApiConfiguration:KeyVaultIdentifier"]), credential);

        services.AddSingleton(certificateClient);

        services.AddHttpClient("SecureClient")
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
                };

                try
                {
                    // Retrieve the certificate from Key Vault
                    var certificateResponse = certificateClient.DownloadCertificate(configuration["LepsLaApiConfiguration:CertificateName"]);
                    var certificate = certificateResponse.Value;

                    if (certificate == null || !certificate.HasPrivateKey)
                    {
                        throw new Exception("❌ The certificate is invalid or does not have a private key.");
                    }

                    // Attach the certificate to HttpClientHandler
                    handler.ClientCertificates.Add(certificate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to load certificate from Key Vault: {ex.Message}");
                    throw;
                }

                return handler;
            });
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient(typeof(IInternalApiClient<>), typeof(InternalApiClient<>));
        services.AddTransient(typeof(ILepsNeApiClient<>), typeof(LepsNeApiClient<>));
        services.AddTransient<IEarlyConnectApiClient<EarlyConnectApiConfiguration>, EarlyConnectApiClient>();
        services.AddTransient<ILepsLoApiClient<LepsLoApiConfiguration>, LepsLoApiClient>();
        services.AddTransient<ILepsLaApiClient<LepsLaApiConfiguration>, LepsLaApiClient>();
        services.AddTransient<IAzureClientCredentialHelper, AzureClientCredentialHelper>();
        services.AddTransient<ISendStudentDataToLepsService, SendStudentDataToLepsService>();
        services.AddFeatureToggle();
    }
}

[ExcludeFromCodeCoverage]
public static class AddConfigurationOptionsExtension
{
    public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<EarlyConnectApiConfiguration>(configuration.GetSection(nameof(EarlyConnectApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<EarlyConnectApiConfiguration>>().Value);

        services.Configure<LepsNeApiConfiguration>(configuration.GetSection(nameof(LepsNeApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LepsNeApiConfiguration>>().Value);

        services.Configure<LepsLoApiConfiguration>(configuration.GetSection(nameof(LepsLoApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LepsLoApiConfiguration>>().Value);

        services.Configure<LepsLaApiConfiguration>(configuration.GetSection(nameof(LepsLaApiConfiguration)));
        services.AddSingleton(cfg => cfg.GetService<IOptions<LepsLaApiConfiguration>>().Value);

        services.Configure<AzureActiveDirectoryConfiguration>(configuration.GetSection("AzureAd"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<AzureActiveDirectoryConfiguration>>().Value);

        services.Configure<EarlyConnectFeaturesConfiguration>(configuration.GetSection("Features"));
        services.AddSingleton<IEarlyConnectFeaturesConfiguration>(cfg => cfg.GetService<IOptions<EarlyConnectFeaturesConfiguration>>().Value);
    }
}