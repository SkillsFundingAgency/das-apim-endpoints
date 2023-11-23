using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
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
        [JsonPropertyName("id")] 
        public long Id { get; set; }

        [JsonPropertyName("anonymousEmployerName")]
        public string AnonymousEmployerName { get; set; }

        [JsonPropertyName("apprenticeshipLevel")]
        public string ApprenticeshipLevel { get; set; }

        [JsonPropertyName("closingDate")] 
        public DateTime ClosingDate { get; set; }

        [JsonPropertyName("employerName")] 
        public string EmployerName { get; set; }

        [JsonPropertyName("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }

        [JsonPropertyName("postedDate")] 
        public DateTime PostedDate { get; set; }

        [JsonPropertyName("title")] 
        public string Title { get; set; }

        [JsonPropertyName("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonPropertyName("course")]
        public Course Course { get; set; }

        [JsonPropertyName("wage")]
        public Wage Wage { get; set; }

        [JsonPropertyName("address")] 
        public Address Address { get; set; }

        [JsonPropertyName("distance")] 
        public decimal? Distance { get; set; }
    }

    public class Address
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
    public class Course
    {
        public int larsCode { get; set; }
        public string title { get; set; }
        public int level { get; set; }
        public string route { get; set; }
    }

    public class Wage
    {
        public double? wageAmount { get; set; }
        public string wageAdditionalInformation { get; set; }
        public string wageType { get; set; }
        public string workingWeekDescription { get; set; }
        public string wageUnit { get; set; }
    }
}
