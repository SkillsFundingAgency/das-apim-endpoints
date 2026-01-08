using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
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

        // Map profession to route
        var liveVacancies = response.Jobs
            .Select(job =>
            {
                var route = GetBusinessRoute(routes.Routes.ToList(), job.Profession.En)
                            ?? routes.Routes.First(r =>
                                r.Name.Equals("Business", StringComparison.OrdinalIgnoreCase));

                return liveVacancyMapper.Map(job, route);
            })
            .ToList();

        // Populate address info in parallel
        var addressTasks = liveVacancies.Select(async vacancy =>
        {
            // Main address
            if (HasValidCoords(vacancy.Address))
            {
                vacancy.Address = await PopulateAddress(vacancy.Address!);
            }

            vacancy.OtherAddresses.AddRange(new List<Address>()
            {
                new Address()
                {
                    AddressLine1 = "23-27 Bolton Street",
                    AddressLine2 = "Chorley",
                    AddressLine4 = "Lancashire",
                    Postcode = "PR7 1JE",
                    Country = "England",
                    Latitude = 53.649653,
                    Longitude = -2.630431
                },
                new Address()
                {
                    AddressLine1 = "Foster House, 2 Victoria Square",
                    AddressLine2 = "Birmingham",
                    AddressLine4 = "West Midlands",
                    Postcode = "B1 1BD",
                    Country = "England",
                    Latitude = 52.478256,
                    Longitude = -1.902689
                },
                new Address()
                {
                    AddressLine1 = "1 St. James's Square",
                    AddressLine2 = "London",
                    AddressLine4 = "Greater London",
                    Postcode = "SW1Y 4AH",
                    Country = "England",
                    Latitude = 51.507351,
                    Longitude = -0.127758
            }});
            vacancy.EmploymentLocationOption = AvailableWhere.MultipleLocations;

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

    public static GetRoutesListItem? GetBusinessRoute(List<GetRoutesListItem> routesListItems, string? profession)
    {
        if (string.IsNullOrWhiteSpace(profession))
            return Default();

        string[] agriculture =
        [
            "Government Veterinary Profession"
        ];

        string[] business =
        [
            "Commercial",
            "Counter Fraud Professions",
            "Government Operational Research Service",
            "Government Project Delivery Profession",
            "Government Statistical Service",
            "Human Resources",
            "International Trade Profession",
            "Knowledge & Information Management",
            "Operational Delivery Profession",
            "Other",
            "Policy Profession"
        ];

        string[] care =
        [
            "Psychology Profession"
        ];

        string[] construction =
        [
            "Government Property Profession",
            "Planning Inspectors",
            "Planning Profession"
        ];

        string[] digital =
        [
            "Government Digital and Data Profession"
        ];

        string[] engineering =
        [
            "Government Science and Engineering"
        ];

        string[] health =
        [
            "Medical Profession"
        ];

        string[] legal =
        [
            "Government Corporate Finance",
            "Government Economic Service",
            "Government Finance",
            "Government Legal Profession",
            "Government Legal Service",
            "Internal Audit",
            "Tax Profession"
        ];

        string[] protectiveServices =
        [
            "Security Profession",
            "Intelligence Analysis"
        ];

        string[] sales =
        [
            "Government Communication Service"
        ];

        if (agriculture.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return FindExact("Agriculture");

        if (business.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Business");

        if (care.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Care");

        if (construction.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Construction");

        if (digital.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return FindExact("Digital");

        if (engineering.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Engineering");

        if (health.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Health");

        if (legal.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Legal");

        if (protectiveServices.Contains(profession, StringComparer.OrdinalIgnoreCase))
            return StartsWith("Protective services");

        return sales.Contains(profession, StringComparer.OrdinalIgnoreCase) 
            ? StartsWith("Sales") 
            : Default();

        GetRoutesListItem? FindExact(string name) =>
            routesListItems.FirstOrDefault(r =>
                r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        GetRoutesListItem? StartsWith(string prefix) =>
            routesListItems.FirstOrDefault(r =>
                r.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

        GetRoutesListItem? Default() => StartsWith("Business");
    }
}