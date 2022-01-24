using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetVacancyResponse
    {
        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }

        [JsonProperty("outcomeDescription")]
        public string OutcomeDescription { get; set; }

        [JsonProperty("trainingDescription")]
        public string TrainingDescription { get; set; }

        [JsonProperty("skills")]
        public List<string> Skills { get; set; }

        [JsonProperty("qualifications")]
        public List<GetVacancyQualificationResponseItem> Qualifications { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("anonymousEmployerName")]
        public string AnonymousEmployerName { get; set; }

        [JsonProperty("apprenticeshipLevel")]
        public string ApprenticeshipLevel { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("categoryCode")]
        public string CategoryCode { get; set; }

        [JsonProperty("closingDate")]
        public DateTime ClosingDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("employerName")]
        public string EmployerName { get; set; }

        [JsonProperty("hoursPerWeek")]
        public decimal HoursPerWeek { get; set; }

        [JsonProperty("isDisabilityConfident")]
        public bool IsDisabilityConfident { get; set; }

        [JsonProperty("isEmployerAnonymous")]
        public bool IsEmployerAnonymous { get; set; }

        [JsonProperty("isPositiveAboutDisability")]
        public bool IsPositiveAboutDisability { get; set; }

        [JsonProperty("isRecruitVacancy")]
        public bool IsRecruitVacancy { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("numberOfPositions")]
        public int NumberOfPositions { get; set; }

        [JsonProperty("postedDate")]
        public DateTime PostedDate { get; set; }

        [JsonProperty("providerName")]
        public string ProviderName { get; set; }

        [JsonProperty("standardLarsCode")]
        public int StandardLarsCode { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("subCategory")]
        public string SubCategory { get; set; }

        [JsonProperty("subCategoryCode")]
        public string SubCategoryCode { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("ukprn")]
        public int Ukprn { get; set; }

        [JsonProperty("vacancyLocationType")]
        public string VacancyLocationType { get; set; }

        [JsonProperty("vacancyReference")]
        public string VacancyReference { get; set; }

        [JsonProperty("wageAmount")]
        public decimal WageAmount { get; set; }

        [JsonProperty("wageAmountLowerBound")]
        public string WageAmountLowerBound { get; set; }

        [JsonProperty("wageAmountUpperBound")]
        public string WageAmountUpperBound { get; set; }

        [JsonProperty("wageText")]
        public string WageText { get; set; }

        [JsonProperty("wageUnit")]
        public int WageUnit { get; set; }

        [JsonProperty("wageType")]
        public int WageType { get; set; }

        [JsonProperty("workingWeek")]
        public string WorkingWeek { get; set; }

        [JsonProperty("distance")]
        public decimal Distance { get; set; }

        [JsonProperty("score")]
        public decimal Score { get; set; }
    }

    public class GetVacancyQualificationResponseItem
    {
        [JsonProperty("weighting")]
        public QualificationWeighting Weighting { get ; set ; }
        [JsonProperty("qualificationType")]
        public string QualificationType { get ; set ; }
        [JsonProperty("subject")]
        public string Subject { get ; set ; }
        [JsonProperty("grade")]
        public string Grade { get ; set ; }
    }
    
    public enum QualificationWeighting
    {
        Essential,
        Desired
    }
}