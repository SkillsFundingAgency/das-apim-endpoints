using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Vacancies.Manage.InnerApi.Requests
{
    public class PostVacancyRequest : IPostApiRequest
    {
        private readonly Guid _id;

        public PostVacancyRequest(Guid id, PostVacancyRequestData postVacancyRequestData)
        {
            _id = id;
            Data = postVacancyRequestData;
        }

        public string PostUrl => $"api/Vacancies/{_id}";
        public object Data { get; set; }
    }

    public class PostVacancyRequestData
    {
        [JsonProperty("title")]
        public string Title { get ; set ; }
        [JsonProperty("description")]
        public string Description { get ; set ; }
        [JsonProperty("programmeId")]
        public string ProgrammeId { get ; set ; }
        [JsonProperty("EmployerAccountId")]
        public string EmployerAccountId { get ; set ; }
        [JsonProperty("user")]
        public VacancyUser User { get ; set ; }
        [JsonProperty("employerName")]
        public string EmployerName { get ; set ; }
        [JsonProperty("shortDescription")]
        public string ShortDescription { get ; set ; }
        [JsonProperty("numberOfPositions")]
        public int NumberOfPositions { get ; set ; }
        [JsonProperty("outcomeDescription")]//TODO check this is required
        public string OutcomeDescription { get ; set ; }
        [JsonProperty("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get ; set ; }
        [JsonProperty("closingDate")]
        public DateTime ClosingDate { get ; set ; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get ; set ; }
        [JsonProperty("legalEntityName")]
        public string LegalEntityName { get ; set ; }
        [JsonProperty("employerDescription")]
        public string EmployerDescription { get ; set ; }
        [JsonProperty("trainingDescription")]
        public string TrainingDescription { get ; set ; }
        [JsonProperty("address")]
        public CreateVacancyAddress Address { get; set; }
        [JsonProperty("wage")]
        public CreateVacancyWage Wage { get; set; }
        [JsonProperty("skills")]
        public List<string> Skills { get ; set ; }
        [JsonProperty("employerNameOption")]
        public EmployerNameOption EmployerNameOption { get ; set ; }
        [JsonProperty("anonymousReason")]
        public string AnonymousReason { get ; set ; }
        [JsonProperty("qualifications")]
        public List<CreateVacancyQualification> Qualifications { get; set; }
        [JsonProperty("applicationInstructions")]
        public string ApplicationInstructions { get ; set ; }
        [JsonProperty("applicationUrl")]
        public string ApplicationUrl { get ; set ; }
        [JsonProperty("applicationMethod")]
        public CreateVacancyApplicationMethod ApplicationMethod { get ; set ; }
        [JsonProperty("disabilityConfident")]
        public CreateVacancyDisabilityConfident DisabilityConfident { get ; set ; }
        [JsonProperty("thingsToConsider")]
        public string ThingsToConsider { get ; set ; }
    }

    public class VacancyUser
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("ukprn")]
        public long? Ukprn { get; set; }
    }
    
    public class CreateVacancyAddress
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

    public class CreateVacancyWage
    {
        [JsonProperty("wageAdditionalInformation")]
        public string WageAdditionalInformation { get ; set ; }
        [JsonProperty("fixedWageYearlyAmount")]
        public decimal? FixedWageYearlyAmount { get ; set ; }
        [JsonProperty("weeklyHours")]
        public decimal WeeklyHours { get ; set ; }
        [JsonProperty("duration")]
        public int Duration { get ; set ; }
        [JsonProperty("workingWeekDescription")]
        public string WorkingWeekDescription { get ; set ; }
        [JsonProperty("wageType")]
        public WageType WageType { get; set; }
        [JsonProperty("durationUnit")]
        public DurationUnit DurationUnit { get; set; }
    }

    public class CreateVacancyQualification
    {
        [JsonProperty("qualificationType")]
        public string QualificationType { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("grade")]
        public string Grade { get; set; }
        [JsonProperty("weighting")]
        public QualificationWeighting Weighting { get; set; }
    }
    
    public enum QualificationWeighting
    {
        Essential,
        Desired
    }
    public enum CreateVacancyApplicationMethod
    {
        ThroughFindAnApprenticeship,
        ThroughExternalApplicationSite
    }
    
    public enum WageType
    {
        FixedWage,
        NationalMinimumWageForApprentices,
        NationalMinimumWage,        
        Unspecified
    }
    
    public enum DurationUnit
    {
        Month,
        Year
    }
    
    public enum EmployerNameOption
    {
        RegisteredName,
        TradingName,
        Anonymous
    }
    public enum CreateVacancyDisabilityConfident
    {
        No = 0,
        Yes
    }
}