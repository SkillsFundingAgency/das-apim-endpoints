using System.Text.Json.Serialization;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public class GetVacanciesResponse
    {
        [JsonPropertyName("apprenticeshipVacancies")]
        public IEnumerable<GetVacanciesListItem> ApprenticeshipVacancies { get; set; } = [];
    }

    public class GetVacanciesListItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("applicationUrl")]
        public string ApplicationUrl { get; set; }

        [JsonPropertyName("closingDate")]
        public DateTime ClosingDate { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("employerName")]
        public string EmployerName { get; set; }
       
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonPropertyName("wageText")]
        public string WageText { get; set; }

        [JsonPropertyName("wageUnit")]
        public int WageUnit { get; set; }

        [JsonPropertyName("wageType")]
        public int WageType { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("isPrimaryLocation")]
        public bool IsPrimaryLocation { get; set; }
        [JsonPropertyName("otherAddresses")]
        public List<Address>? OtherAddresses { get; set; }
        [JsonPropertyName("employmentLocationInformation")]
        public string? EmploymentLocationInformation { get; set; }

        [JsonPropertyName("availableWhere"), JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
        public AvailableWhere? EmploymentLocationOption { get; set; }

        [JsonPropertyName("distance")]
        public decimal? Distance { get; set; }

        [JsonPropertyName("standardTitle")]
        public string CourseTitle { get; set; }

        [JsonPropertyName("standardLevel")]
        public string CourseLevel { get; set; }
      
        [JsonPropertyName("vacancySource")]
        public string VacancySource { get; set; }
        
        [JsonPropertyName("apprenticeshipType")]
        public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    }
}
