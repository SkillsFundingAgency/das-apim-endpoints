using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Campaign.InnerApi.Responses
{
    public class GetVacanciesResponse
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("totalFound")]
        public long TotalFound { get; set; }

        [JsonPropertyName("apprenticeshipVacancies")]
        public IEnumerable<GetVacanciesListItem> ApprenticeshipVacancies { get; set; }
    }

    public class GetVacanciesListItem
    {
        [JsonPropertyName("anonymousEmployerName")]
        public string AnonymousEmployerName { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("subCategory")]
        public string SubCategory { get; set; }
        
        [JsonPropertyName("closingDate")]
        public DateTime ClosingDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("employerName")]
        public string EmployerName { get; set; }

        [JsonPropertyName("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }
        
        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("postedDate")]
        public DateTime PostedDate { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonPropertyName("distance")]
        public decimal? Distance { get; set; }
        [JsonPropertyName("standardLarsCode")]
        public int? StandardLarsCode { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string VacancyUrl { get ; set ; }
    }
    
    public class Location
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }
    
}