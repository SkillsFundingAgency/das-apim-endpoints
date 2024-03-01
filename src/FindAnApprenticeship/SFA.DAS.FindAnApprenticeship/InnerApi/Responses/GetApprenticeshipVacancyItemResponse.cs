using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.Responses
{
    public class GetApprenticeshipVacancyItemResponse : GetVacanciesListItem
    {
        [JsonProperty("longDescription")]
        public string LongDescription { get; init; }
        [JsonProperty("outcomeDescription")]
        public string OutcomeDescription { get; init; }
        [JsonProperty("trainingDescription")]
        public string TrainingDescription { get; init; }
        [JsonProperty("thingsToConsider")]
        public string ThingsToConsider { get; init; }
        [JsonProperty("category")]
        public string Category { get; init; }
        [JsonProperty("categoryCode")]
        public string CategoryCode { get; init; }
        [JsonProperty("description")]
        public string Description { get; init; }
        [JsonProperty("frameworkLarsCode")]
        public string FrameworkLarsCode { get; init; }
        [JsonProperty("hoursPerWeek")]
        public decimal? HoursPerWeek { get; init; }
        [JsonProperty("isDisabilityConfident")]
        public bool IsDisabilityConfident { get; init; }
        [JsonProperty("isPositiveAboutDisability")]
        public bool IsPositiveAboutDisability { get; init; }
        [JsonProperty("isRecruitVacancy")]
        public bool IsRecruitVacancy { get; init; }
        [JsonProperty("location")]
        public GeoPoint Location { get; init; }
        [JsonProperty("numberOfPositions")]
        public int NumberOfPositions { get; init; }
        [JsonProperty("providerName")]
        public string ProviderName { get; init; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; init; }
        [JsonProperty("subCategory")]
        public string SubCategory { get; init; }
        [JsonProperty("subCategoryCode")]
        public string SubCategoryCode { get; init; }
        [JsonProperty("ukprn")]
        public string Ukprn { get; init; }
        [JsonProperty("wageAmountLowerBound")]
        public decimal? WageAmountLowerBound { get; init; }
        [JsonProperty("wageAmountUpperBound")]
        public decimal? WageAmountUpperBound { get; init; }
        [JsonProperty("wageText")]
        public string WageText { get; init; }
        [JsonProperty("wageUnit")]
        public int WageUnit { get; init; }
        [JsonProperty("workingWeek")]
        public string WorkingWeek { get; init; }
        [JsonProperty("expectedDuration")]
        public string ExpectedDuration { get; init; }
        [JsonProperty("score")]
        public double Score { get; init; }
        [JsonProperty("employerDescription")]
        public string EmployerDescription { get; init; }
        [JsonProperty("employerWebsiteUrl")]
        public string EmployerWebsiteUrl { get; init; }
        [JsonProperty("employerContactPhone")]
        public string EmployerContactPhone { get; init; }
        [JsonProperty("employerContactEmail")]
        public string EmployerContactEmail { get; init; }
        [JsonProperty("employerContactName")]
        public string EmployerContactName { get; init; }

        [JsonProperty("vacancyLocationType")]
        public VacancyLocationType VacancyLocationType { get; init; }

        [JsonProperty("skills")]
        public IEnumerable<string> Skills { get; init; }
        [JsonProperty("qualifications")]
        public IEnumerable<VacancyQualification> Qualifications { get; init; }

        [JsonProperty("additionalQuestion1")]
        public string AdditionalQuestion1 { get; init; }

        [JsonProperty("additionalQuestion2")]
        public string AdditionalQuestion2 { get; init; }

    }

    public class VacancyQualification
    {
        [JsonProperty("qualificationType")]
        public string QualificationType { get; init; }
        [JsonProperty("subject")]
        public string Subject { get; init; }
        [JsonProperty("grade")]
        public string Grade { get; init; }
        [JsonProperty("weighting")]
        public Weighting Weighting { get; init; }
    }

    public enum Weighting
    {
        Essential,
        Desired
    }

    public enum VacancyLocationType
    {
        Unknown = 0,
        NonNational,
        National
    }
}