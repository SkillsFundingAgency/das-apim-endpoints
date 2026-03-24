using System.Net;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.ExternalApi.DfeSignIn;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.DfeSignIn;

namespace SFA.DAS.Aodp.Services;

public sealed class DfeUsersService : IDfeUsersService
{
    private readonly IDfeUsersApiClient<DfeSignInApiConfiguration> _client;
    private readonly DfeSignInApiConfiguration _cfg;
    private readonly ILogger<DfeUsersService> _logger;

    public DfeUsersService(
        IDfeUsersApiClient<DfeSignInApiConfiguration> client,
        DfeSignInApiConfiguration cfg,
        ILogger<DfeUsersService> logger)
    {
        _client = client;
        _cfg = cfg;
        _logger = logger;
    }

    public async Task<IReadOnlyList<User>> GetUsersByRoleAsync(
        string ukprn,
        string role,
        CancellationToken ct = default)
    {
        try
        {
            var host = new Uri(_cfg.Url).Host;
            var resolvedIps = await Dns.GetHostAddressesAsync(host, ct);

            _logger.LogInformation(
                "Calling DfE users API. Url: {Url}, Host: {Host}, Audience: {Audience}, ClientId: {ClientId}, Ukprn: {Ukprn}, Role: {Role}, TokenLifetimeMinutes: {TokenLifetimeMinutes}, ResolvedIPs: {ResolvedIPs}, WebsiteSiteName: {WebsiteSiteName}, WebsiteInstanceId: {WebsiteInstanceId}, OutboundIPs: {OutboundIPs}",
                _cfg.Url,
                host,
                _cfg.Audience,
                _cfg.ClientId,
                ukprn,
                role,
                _cfg.TokenLifetimeMinutes,
                string.Join(",", resolvedIps.Select(x => x.ToString())),
                Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"),
                Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"),
                Environment.GetEnvironmentVariable("WEBSITE_OUTBOUND_IP_ADDRESSES"));

            var response = await _client.Get<OrganisationUsersResponse>(
                new GetUsersByRoleApiRequest(ukprn, role));

            var users = response?.Users?.ToList() ?? new List<User>();

            _logger.LogInformation(
                "DfE users API call succeeded. Ukprn: {Ukprn}, Role: {Role}, UserCount: {UserCount}",
                ukprn,
                role,
                users.Count);

            return users;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "DfE users API call failed. Url: {Url}, Audience: {Audience}, ClientId: {ClientId}, Ukprn: {Ukprn}, Role: {Role}, WebsiteSiteName: {WebsiteSiteName}, WebsiteInstanceId: {WebsiteInstanceId}, OutboundIPs: {OutboundIPs}",
                _cfg.Url,
                _cfg.Audience,
                _cfg.ClientId,
                ukprn,
                role,
                Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"),
                Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"),
                Environment.GetEnvironmentVariable("WEBSITE_OUTBOUND_IP_ADDRESSES"));

            throw new ApplicationException("Unable to retrieve reviewer users from DfE Sign-in.", ex);
        }
    }
}