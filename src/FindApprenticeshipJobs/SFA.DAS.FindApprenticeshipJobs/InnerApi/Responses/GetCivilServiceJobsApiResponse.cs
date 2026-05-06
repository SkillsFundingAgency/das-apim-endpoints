using Newtonsoft.Json;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
public record GetCivilServiceJobsApiResponse
{
    [JsonProperty("jobs")]
    
    public List<Job> Jobs { get; set; } = [];

    public record Job
    {
        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("jobAdvertOnly")]
        public bool JobAdvertOnly { get; set; }

        [JsonProperty("jobApplyURL")]
        public JobApplyUrl JobApplyUrl { get; set; }

        [JsonProperty("jobCode")]
        public string? JobCode { get; set; }

        [JsonProperty("jobReference")]
        public string? JobReference { get; set; }

        [JsonProperty("jobSystem")]
        public string? JobSystem { get; set; }

        [JsonProperty("jobTitle")]
        public JobTitle JobTitle { get; set; }

        [JsonProperty("jobURL")]
        public string? JobUrl { get; set; }

        [JsonProperty("keyTimes")]
        public KeyTimes KeyTimes { get; set; }

        [JsonProperty("approach")]
        public Approach Approach { get; set; }

        [JsonProperty("contractType")]
        public ContractType ContractType { get; set; }

        [JsonProperty("countryRegions")]
        public CountryRegions CountryRegions { get; set; }

        [JsonProperty("department")]
        public Department Department { get; set; }

        [JsonProperty("grade")]
        public Grade Grade { get; set; }

        [JsonProperty("profession")]
        public Profession Profession { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("workingPattern")]
        public WorkingPattern WorkingPattern { get; set; }

        [JsonProperty("locationFlags")]
        public LocationFlags LocationFlags { get; set; }

        [JsonProperty("locationGeoCoordinates")]
        public List<LocationGeoCoordinate> LocationGeoCoordinates { get; set; } = [];

        [JsonProperty("salaryCurrency")]
        public string? SalaryCurrency { get; set; }

        [JsonProperty("salaryMaximum")]
        public decimal SalaryMaximum { get; set; }

        [JsonProperty("salaryMinimum")]
        public decimal SalaryMinimum { get; set; }
    }

    public class Approach
    {
        [JsonProperty("en")]
        public string En { get; set; }
    }

    public class ContractType
    {
        [JsonProperty("en")] 
        public List<string> En { get; set; } = [];
    }

    public class Country
    {
        [JsonProperty("en")]
        public string? En { get; set; }
    }

    public class CountryRegions
    {
        [JsonProperty("en")]
        public List<string> En { get; set; } = [];
    }

    public class Department
    {
        [JsonProperty("en")]
        public string? En { get; set; }
    }

    public class Grade
    {
        [JsonProperty("en")]
        public List<string> En { get; set; } = [];
    }
   
    public class JobApplyUrl
    {
        [JsonProperty("en")]
        public string? En { get; set; }
    }

    public class JobTitle
    {
        [JsonProperty("en")]
        public string? En { get; set; }
    }

    public class KeyTimes
    {
        [JsonProperty("closingTime")]
        public DateTime ClosingTime { get; set; }

        [JsonProperty("publishedTime")]
        public DateTime PublishedTime { get; set; }

        [JsonProperty("updatedTime")]
        public DateTime UpdatedTime { get; set; }
    }

    public class LocationFlags
    {
        [JsonProperty("countryRegions")]
        public bool CountryRegions { get; set; }

        [JsonProperty("locationGeoCoordinates")]
        public bool LocationGeoCoordinates { get; set; }

        [JsonProperty("overseas")]
        public bool Overseas { get; set; }

        [JsonProperty("remoteWorking")]
        public bool RemoteWorking { get; set; }
    }

    public class LocationGeoCoordinate
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }

    public class Meta
    {
        [JsonProperty("numberOfJobs")]
        public int NumberOfJobs { get; set; }
    }

    public class Profession
    {
        [JsonProperty("en")]
        public string? En { get; set; }
    }

    public class Role
    {
        [JsonProperty("en")]
        public List<string> En { get; set; } = [];
    }

    public class WorkingPattern
    {
        [JsonProperty("en")]
        public List<string> En { get; set; } = [];
    }
}
