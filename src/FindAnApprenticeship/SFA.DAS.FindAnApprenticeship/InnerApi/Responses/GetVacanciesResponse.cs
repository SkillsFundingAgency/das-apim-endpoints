using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.Vacancies.InnerApi.Responses;

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

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("categoryCode")]
        public string CategoryCode { get; set; }

        [JsonPropertyName("closingDate")]
        public DateTime ClosingDate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("employerName")]
        public string EmployerName { get; set; }

        [JsonPropertyName("frameworkLarsCode")]
        public string FrameworkLarsCode { get; set; }

        [JsonPropertyName("hoursPerWeek")]
        public decimal HoursPerWeek { get; set; }

        [JsonPropertyName("isDisabilityConfident")]
        public bool IsDisabilityConfident { get; set; }

        [JsonPropertyName("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }

        [JsonPropertyName("isPositiveAboutDisability")]
        public bool IsPositiveAboutDisability { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("numberOfPositions")]
        public long NumberOfPositions { get; set; }

        [JsonPropertyName("postedDate")]
        public DateTime PostedDate { get; set; }

        [JsonPropertyName("providerName")]
        public string ProviderName { get; set; }

        [JsonPropertyName("standardLarsCode")]
        public int? StandardLarsCode { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("subCategory")]
        public string SubCategory { get; set; }

        [JsonPropertyName("subCategoryCode")]
        public string SubCategoryCode { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("ukprn")]
        public int Ukprn { get; set; }

        [JsonPropertyName("vacancyLocationType")]
        public string VacancyLocationType { get; set; }

        [JsonPropertyName("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonPropertyName("wageAmount")]
        public decimal? WageAmount { get; set; }

        [JsonPropertyName("wageAmountLowerBound")]
        public decimal? WageAmountLowerBound { get; set; }

        [JsonPropertyName("wageAmountUpperBound")]
        public decimal? WageAmountUpperBound { get; set; }

        [JsonPropertyName("wageText")]
        public string WageText { get; set; }

        [JsonPropertyName("wageUnit")]
        public int WageUnit { get; set; }

        [JsonPropertyName("wageType")]
        public int WageType { get; set; }

        [JsonPropertyName("workingWeek")]
        public string WorkingWeek { get; set; }
        
        [JsonPropertyName("expectedDuration")]
        public string ExpectedDuration { get; set; }
        
        [JsonPropertyName("employerWebsiteUrl")]
        public string EmployerWebsiteUrl { get; set; }

        [JsonPropertyName("employerContactPhone")]
        public string EmployerContactPhone { get; set; }
        
        [JsonPropertyName("employerContactEmail")]
        public string EmployerContactEmail { get; set; }
        
        [JsonPropertyName("employerContactName")]
        public string EmployerContactName { get; set; }
        
        [JsonPropertyName("address")]
        public Address Address { get; set; }
        
        [JsonPropertyName("distance")]
        public decimal? Distance { get; set; }

        [JsonPropertyName("score")]
        public long Score { get; set; }
        
        [JsonIgnore]
        public string CourseTitle { get ; set ; }
        [JsonIgnore]
        public string Route { get ; set ; }
        [JsonIgnore]
        public int CourseLevel { get ; set ; }
        [JsonIgnore]
        public string VacancyUrl { get ; set ; }
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
}
