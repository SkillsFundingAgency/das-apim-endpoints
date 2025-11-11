using MediatR;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using static SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses.GetCivilServiceJobsApiResponse;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
public class GetCivilServiceJobsQueryHandler(
    ILiveVacancyMapper liveVacancyMapper,
    ICivilServiceJobsApiClient<CivilServiceJobsApiConfiguration> civilServiceJobsApiClient,
    ILocationApiClient<LocationApiConfiguration> locationApiClient,
    ICourseService courseService) : IRequestHandler<GetCivilServiceJobsQuery, GetCivilServiceJobsQueryResult>
{
    public async Task<GetCivilServiceJobsQueryResult> Handle(GetCivilServiceJobsQuery request, CancellationToken cancellationToken)
    {
        //var response = await civilServiceJobsApiClient.Get<GetCivilServiceJobsApiResponse>(new GetCivilServiceJobsApiRequest());

        var response = new GetCivilServiceJobsApiResponse()
        {
            Jobs = new List<Job>
            {
                new Job
                {
                    Country = new Country {En = "United Kingdom"},
                    JobAdvertOnly = false,
                    JobApplyUrl = new JobApplyUrl {En = "https://www.civilservicejobs.service.gov.uk/job/12345"},
                    JobCode = "CSJ-2025-001",
                    JobReference = "REF123456",
                    JobSystem = "CSJobs",
                    JobTitle = new JobTitle {En = "Software Engineer"},
                    JobUrl = "https://www.civilservicejobs.service.gov.uk/job/12345",
                    KeyTimes = new KeyTimes
                    {
                        PublishedTime = DateTime.UtcNow.AddDays(-5),
                        UpdatedTime = DateTime.UtcNow.AddDays(-1),
                        ClosingTime = DateTime.UtcNow.AddDays(7)
                    },
                    Approach = new Approach {En = "External"},
                    ContractType = new ContractType {En = new List<string> {"Permanent", "Full-time"}},
                    CountryRegions = new CountryRegions {En = new List<string> {"England", "Scotland"}},
                    Department = new Department {En = "Department for Education"},
                    Grade = new Grade {En = new List<string> {"Grade 7"}},
                    Profession = new Profession {En = "Digital, Data and Technology"},
                    Role = new Role {En = new List<string> {"Developer", "Software Engineer"}},
                    WorkingPattern = new WorkingPattern {En = new List<string> {"Flexible working", "Homeworking"}},
                    LocationFlags = new LocationFlags
                    {
                        CountryRegions = true,
                        LocationGeoCoordinates = true,
                        Overseas = false,
                        RemoteWorking = true
                    },
                    LocationGeoCoordinates = new List<LocationGeoCoordinate>
                    {
                        new LocationGeoCoordinate {Lat = 51.5074, Lon = -0.1278}, // London
                        new LocationGeoCoordinate {Lat = 53.4808, Lon = -2.2426} // Manchester
                    },
                    SalaryCurrency = "GBP",
                    SalaryMaximum = 65000m,
                    SalaryMinimum = 45000m
                },
                new Job
                {
                    Country = new Country {En = "United Kingdom"},
                    JobAdvertOnly = true,
                    JobApplyUrl = new JobApplyUrl {En = "https://www.civilservicejobs.service.gov.uk/job/98765"},
                    JobCode = "CSJ-2025-002",
                    JobReference = "REF987654",
                    JobSystem = "CSJobs",
                    JobTitle = new JobTitle {En = "Policy Advisor"},
                    JobUrl = "https://www.civilservicejobs.service.gov.uk/job/98765",
                    KeyTimes = new KeyTimes
                    {
                        PublishedTime = DateTime.UtcNow.AddDays(-10),
                        UpdatedTime = DateTime.UtcNow.AddDays(-2),
                        ClosingTime = DateTime.UtcNow.AddDays(5)
                    },
                    Approach = new Approach {En = "Internal"},
                    ContractType = new ContractType {En = new List<string> {"Fixed Term", "Part-time"}},
                    CountryRegions = new CountryRegions {En = new List<string> {"Wales"}},
                    Department = new Department {En = "Cabinet Office"},
                    Grade = new Grade {En = new List<string> {"HEO"}},
                    Profession = new Profession {En = "Policy"},
                    Role = new Role {En = new List<string> {"Advisor"}},
                    WorkingPattern = new WorkingPattern {En = new List<string> {"Part-time"}},
                    LocationFlags = new LocationFlags
                    {
                        CountryRegions = true,
                        LocationGeoCoordinates = false,
                        Overseas = false,
                        RemoteWorking = false
                    },
                    LocationGeoCoordinates = new List<LocationGeoCoordinate>
                    {
                        new LocationGeoCoordinate {Lat = 51.4816, Lon = -3.1791} // Cardiff
                    },
                    SalaryCurrency = "GBP",
                    SalaryMaximum = 42000m,
                    SalaryMinimum = 30000m
                }
            }
        };

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
            //var locationApiResponse = await locationApiClient.Get<GetAddressByCoordinatesApiResponse>(
            //    new GetAddressByCoordinatesApiRequest((double)liveVacancy.Address.Latitude!, (double)liveVacancy.Address.Longitude!));

            //if (locationApiResponse == null) continue;

            // Update the live vacancy address with the location API response
            liveVacancy.Address.AddressLine1 = "locationApiResponse.AddressLine1";
            liveVacancy.Address.AddressLine2 = "locationApiResponse.AddressLine2";
            liveVacancy.Address.AddressLine3 = "locationApiResponse.AddressLine3";
            liveVacancy.Address.Postcode = "WC2N 5DU";
            liveVacancy.Address.Country = "England";
        }

        return new GetCivilServiceJobsQueryResult
        {
            CivilServiceVacancies = liveVacancies
        };
    }
}