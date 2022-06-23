using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.InnerApi.Responses
{
    public class GetVacanciesResponse
    {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("totalFound")]
        public long TotalFound { get; set; }

        [JsonProperty("apprenticeshipVacancies")]
        public IEnumerable<GetVacanciesListItem> ApprenticeshipVacancies { get; set; }
    }

    public class GetVacanciesListItem
    {
        [JsonProperty("anonymousEmployerName")]
        public string AnonymousEmployerName { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subCategory")]
        public string SubCategory { get; set; }
        
        [JsonProperty("closingDate")]
        public DateTime ClosingDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("employerName")]
        public string EmployerName { get; set; }

        [JsonProperty("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }
        
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("postedDate")]
        public DateTime PostedDate { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonProperty("distance")]
        public decimal? Distance { get; set; }
        [JsonProperty("standardLarsCode")]
        public int? StandardLarsCode { get; set; }

        [JsonIgnore]
        public string VacancyUrl { get ; set ; }
    }
    
    public class Location
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
    
}