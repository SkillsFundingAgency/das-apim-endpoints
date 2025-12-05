using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

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

        if (response?.Jobs is null || response.Jobs.Count == 0)
        {
            return new GetCivilServiceJobsQueryResult
            {
                CivilServiceVacancies = []
            };
        }

        var routes = await courseService.GetRoutes();
        var route = routes.Routes
            .FirstOrDefault(c => c.Name.Contains("Business", StringComparison.OrdinalIgnoreCase));

        var liveVacancies = response.Jobs
            .Select(c => liveVacancyMapper.Map(c, route))
            .ToList();

        // Populate address info in parallel
        var addressTasks = liveVacancies.Select(async vacancy =>
        {
            // Main address
            if (HasValidCoords(vacancy.Address))
            {
                vacancy.Address = await PopulateAddress(vacancy.Address!);
            }

            // Other addresses
            if (vacancy.OtherAddresses is {Count: > 0})
            {
                var tasks = vacancy.OtherAddresses
                    .Where(HasValidCoords)
                    .Select(async addr =>
                    {
                        var updated = await PopulateAddress(addr);
                        return (Original: addr, Updated: updated);
                    });

                var results = await Task.WhenAll(tasks);

                // Replace original instances
                foreach (var (orig, updated) in results)
                {
                    var idx = vacancy.OtherAddresses.IndexOf(orig);
                    if (idx >= 0) vacancy.OtherAddresses[idx] = updated;
                }
            }
        });

        await Task.WhenAll(addressTasks);

        return new GetCivilServiceJobsQueryResult
        {
            CivilServiceVacancies = liveVacancies
        };
    }

    private static bool HasValidCoords(Address? address)
    {
        return  address is {Latitude: not null, Longitude: not null} 
                && address.Latitude != 0 && address.Longitude != 0;
    }

    private async Task<Address> PopulateAddress(Address address)
    {
        var response = await locationApiClient.Get<GetAddressByCoordinatesApiResponse>(
            new GetAddressByCoordinatesApiRequest(
                (double)address.Latitude!,
                (double)address.Longitude!));

        if (response is null)
            return address;

        address.AddressLine1 = response.AddressLine1;
        address.AddressLine2 = response.AddressLine2;
        address.AddressLine3 = response.AddressLine3;
        address.Postcode = response.Postcode;
        address.Country = response.Country;

        return address;
    }
}