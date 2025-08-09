using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
public class GetCivilServiceJobsQueryHandler(
    IOptions<CivilServiceJobsConfiguration> civilServiceJobsConfiguration,
    ICivilServiceJobsApiClient apiClient,
    ILiveVacancyMapper liveVacancyMapper,
    ILocationApiClient<LocationApiConfiguration> locationApiClient,
    ICourseService courseService,
    ILogger<GetCivilServiceJobsQueryHandler> logger) : IRequestHandler<GetCivilServiceJobsQuery, GetCivilServiceJobsQueryResult>
{
    private readonly CivilServiceJobsConfiguration _civilServiceJobsConfiguration = civilServiceJobsConfiguration.Value;

    public async Task<GetCivilServiceJobsQueryResult> Handle(GetCivilServiceJobsQuery request, CancellationToken cancellationToken)
    {
        // Ensure the API client is configured with the correct base address
        logger.LogInformation("CSJ API Url:{url}", _civilServiceJobsConfiguration.Url);
        // Log the API key, truncating it for security
        logger.LogInformation("CSJ API Key:{key}", TruncateGuid(_civilServiceJobsConfiguration.ApiKey));
        if (string.IsNullOrWhiteSpace(_civilServiceJobsConfiguration.Url) || string.IsNullOrWhiteSpace(_civilServiceJobsConfiguration.ApiKey))
        {
            logger.LogError("Civil Service Jobs API configuration is not set correctly.");
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }
        // Fetch the civil service jobs from the API
        logger.LogInformation("Fetching Civil Service Jobs from API");
        logger.LogInformation("CSJ API Request: {request}", new GetCivilServiceJobsApiRequest().GetUrl);

        var outboundIp = await LogOutboundIpAsync();
        logger.LogInformation("Outbound IP for CSJ API request: {ip}", outboundIp);

        var response = await apiClient.GetWithResponseCode(new GetCivilServiceJobsApiRequest());

        logger.LogInformation("CSJ Response code from API: {response}", response.StatusCode);
        logger.LogInformation("CSJ Response Error from API: {response}", response.ErrorContent);
        logger.LogInformation("CSJ Response from API: {response}", response.Body);
        
        if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Body is null)
        {
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }
        
        var civilServiceJobsApiResponse = JsonConvert.DeserializeObject<GetCivilServiceJobsApiResponse>(response.Body);
        if (civilServiceJobsApiResponse == null || civilServiceJobsApiResponse.Jobs.Count == 0)
        {
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }

        var routes = await courseService.GetRoutes();
        var route = routes.Routes.FirstOrDefault(c => c.Name.Contains("Business", StringComparison.CurrentCultureIgnoreCase));
        var liveVacancies = civilServiceJobsApiResponse.Jobs.Select(c => liveVacancyMapper.Map(c, route)).ToList();

        foreach (var liveVacancy in liveVacancies)
        {
            if (liveVacancy.Address?.Latitude == null ||
                liveVacancy.Address.Longitude == null ||
                liveVacancy.Address.Latitude == 0 ||
                liveVacancy.Address.Longitude == 0)
            {
                continue; // Skip if address is not available or coordinates are not set
            }
            
            // Fetch address details using the coordinates
            var locationApiResponse = await locationApiClient.Get<GetAddressByCoordinatesApiResponse>(
                new GetAddressByCoordinatesApiRequest((double)liveVacancy.Address.Latitude!, (double)liveVacancy.Address.Longitude!));

            if (locationApiResponse == null) continue;

            // Update the live vacancy address with the location API response
            liveVacancy.Address.AddressLine1 = locationApiResponse.AddressLine1;
            liveVacancy.Address.AddressLine2 = locationApiResponse.AddressLine2;
            liveVacancy.Address.AddressLine3 = locationApiResponse.AddressLine3;
            liveVacancy.Address.Postcode = locationApiResponse.Postcode;
            liveVacancy.Address.Country = locationApiResponse.Country;
        }

        return new GetCivilServiceJobsQueryResult
        {
            CivilServiceVacancies = liveVacancies
        };
    }

    public static async Task<string> LogOutboundIpAsync()
    {
        using var client = new HttpClient();
        var ip = await client.GetStringAsync("https://api.ipify.org");
        return ip;
    }

    static string TruncateGuid(string guid)
    {
        string full = guid.ToString();
        return $"{full[..8]}-****-****-****-******{full[^4..]}";
    }
}
