using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostTraineeshipVacancyRequestData
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("routeId")]
        public int RouteId { get; set; }
        [JsonProperty("EmployerAccountId")]
        public string EmployerAccountId { get; set; }
        [JsonProperty("user")]
        public PostTraineeshipVacancyUserData User { get; set; }
        [JsonProperty("employerName")]
        public string EmployerName { get; set; }
        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }
        [JsonProperty("numberOfPositions")]
        public int NumberOfPositions { get; set; }
        [JsonProperty("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get; set; }
        [JsonProperty("closingDate")]
        public DateTime ClosingDate { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("legalEntityName")]
        public string LegalEntityName { get; set; }
        [JsonProperty("employerDescription")]
        public string EmployerDescription { get; set; }
        [JsonProperty("trainingDescription")]
        public string TrainingDescription { get; set; }
        [JsonProperty("address")]
        public PostTraineeshipVacancyAddressData Address { get; set; }
        [JsonProperty("wage")]
        public PostCreateTraineeshipVacancyWageData Wage { get; set; }
        [JsonProperty("skills")]
        public List<string> Skills { get; set; }
        [JsonProperty("employerNameOption")]
        public TraineeshipEmployerNameOption EmployerNameOption { get; set; }
        [JsonProperty("anonymousReason")]
        public string AnonymousReason { get; set; }
        [JsonProperty("disabilityConfident")]
        public CreateTraineeshipVacancyDisabilityConfident DisabilityConfident { get; set; }
        [JsonProperty("outcomeDescription")]
        public string OutcomeDescription { get; set; }

        [JsonProperty("workExperience")]
        public string WorkExperience { get; set; }
    }

    public class PostTraineeshipVacancyUserData
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("ukprn")]
        public int Ukprn { get; set; }
    }

    public class PostTraineeshipVacancyAddressData
    {
        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty("addressLine3")]
        public string AddressLine3 { get; set; }
        [JsonProperty("addressLine4")]
        public string AddressLine4 { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }

    public class PostCreateTraineeshipVacancyWageData
    {
        [JsonProperty("weeklyHours")]
        public decimal WeeklyHours { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("workingWeekDescription")]
        public string WorkingWeekDescription { get; set; }
        [JsonProperty("wageType")]
        public WageType WageType { get; set; }
        [JsonProperty("durationUnit")]
        public DurationUnit DurationUnit { get; set; }
    }
    public enum TraineeshipDurationUnit
    {
        Month,
        Year
    }

    public enum TraineeshipEmployerNameOption
    {
        RegisteredName,
        TradingName,
        Anonymous
    }
    public enum CreateTraineeshipVacancyDisabilityConfident
    {
        No = 0,
        Yes
    }
}