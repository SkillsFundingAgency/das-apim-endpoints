using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
public class GetCivilServiceJobsQueryHandler(
    ILiveVacancyMapper liveVacancyMapper,
    ICivilServiceJobsApiClient<CivilServiceJobsApiConfiguration> civilServiceJobsApiClient,
    ILocationApiClient<LocationApiConfiguration> locationApiClient,
    ICourseService courseService) : IRequestHandler<GetCivilServiceJobsQuery, GetCivilServiceJobsQueryResult>
{
    public async Task<GetCivilServiceJobsQueryResult> Handle(GetCivilServiceJobsQuery request, CancellationToken cancellationToken)
    {
        var response = await civilServiceJobsApiClient.Get<GetCivilServiceJobsApiResponse>(new GetCivilServiceJobsApiRequest());

        if (response.Jobs.Count == 0)
        {
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }

        var routes = await courseService.GetRoutes();
        var route = routes.Routes.FirstOrDefault(c => c.Name.Contains("Business", StringComparison.CurrentCultureIgnoreCase));
        var liveVacancies = response.Jobs.Select(c => liveVacancyMapper.Map(c, route)).ToList();

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
}