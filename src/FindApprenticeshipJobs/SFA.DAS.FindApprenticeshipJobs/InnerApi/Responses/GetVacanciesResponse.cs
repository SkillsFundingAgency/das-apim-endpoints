using System.Text.Json.Serialization;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses
{
    public class GetVacanciesResponse
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("totalFound")]
        public long TotalFound { get; set; }

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

        [JsonPropertyName("employerName")]
        public string EmployerName { get; set; }
       
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonPropertyName("wageText")]
        public string WageText { get; set; }

        [JsonPropertyName("address")]
        public VacancyAddress VacancyAddress { get; set; }

        [JsonPropertyName("distance")]
        public decimal? Distance { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("standardTitle")]
        public string CourseTitle { get; set; }

        [JsonPropertyName("standardLevel")]
        public int CourseLevel { get; set; }

        [JsonPropertyName("vacancySource")]
        public string VacancySource { get; set; }
    }

    public class VacancyAddress
    {
        [JsonPropertyName("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonPropertyName("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonPropertyName("addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonPropertyName("addressLine4")]
        public string AddressLine4 { get; set; }
        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }
    }
}
