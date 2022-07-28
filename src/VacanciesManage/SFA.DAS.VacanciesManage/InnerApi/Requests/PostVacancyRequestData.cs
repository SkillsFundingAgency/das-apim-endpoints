using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.VacanciesManage.InnerApi.Requests
{
    public class PostVacancyRequestData
    {
        [JsonPropertyName("title")]
        public string Title { get ; set ; }
        [JsonPropertyName("description")]
        public string Description { get ; set ; }
        [JsonPropertyName("programmeId")]
        public string ProgrammeId { get ; set ; }
        [JsonPropertyName("EmployerAccountId")]
        public string EmployerAccountId { get ; set ; }
        [JsonPropertyName("user")]
        public PostVacancyUserData User { get ; set ; }
        [JsonPropertyName("employerName")]
        public string EmployerName { get ; set ; }
        [JsonPropertyName("shortDescription")]
        public string ShortDescription { get ; set ; }
        [JsonPropertyName("numberOfPositions")]
        public int NumberOfPositions { get ; set ; }
        [JsonPropertyName("accountLegalEntityPublicHashedId")]
        public string AccountLegalEntityPublicHashedId { get ; set ; }
        [JsonPropertyName("closingDate")]
        public DateTime ClosingDate { get ; set ; }
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get ; set ; }
        [JsonPropertyName("legalEntityName")]
        public string LegalEntityName { get ; set ; }
        [JsonPropertyName("employerDescription")]
        public string EmployerDescription { get ; set ; }
        [JsonPropertyName("trainingDescription")]
        public string TrainingDescription { get ; set ; }
        [JsonPropertyName("address")]
        public PostVacancyAddressData Address { get; set; }
        [JsonPropertyName("wage")]
        public PostCreateVacancyWageData Wage { get; set; }
        [JsonPropertyName("skills")]
        public List<string> Skills { get ; set ; }
        [JsonPropertyName("employerNameOption")]
        public EmployerNameOption EmployerNameOption { get ; set ; }
        [JsonPropertyName("anonymousReason")]
        public string AnonymousReason { get ; set ; }
        [JsonPropertyName("qualifications")]
        public List<PostCreateVacancyQualificationData> Qualifications { get; set; }
        [JsonPropertyName("applicationInstructions")]
        public string ApplicationInstructions { get ; set ; }
        [JsonPropertyName("applicationUrl")]
        public string ApplicationUrl { get ; set ; }
        [JsonPropertyName("applicationMethod")]
        public CreateVacancyApplicationMethod ApplicationMethod { get ; set ; }
        [JsonPropertyName("disabilityConfident")]
        public CreateVacancyDisabilityConfident DisabilityConfident { get ; set ; }
        [JsonPropertyName("thingsToConsider")]
        public string ThingsToConsider { get ; set ; }
        [JsonPropertyName("outcomeDescription")]
        public string OutcomeDescription { get ; set ; }
        [JsonPropertyName("employerWebsiteUrl")]
        public string EmployerWebsiteUrl { get; set; }
        [JsonPropertyName("employerContact")]
        public ContactDetails EmployerContact { get; set; }
        [JsonPropertyName("providerContact")]
        public ContactDetails ProviderContact { get; set; }
        [JsonPropertyName("ownerType")]
        public OwnerType OwnerType { get ; set ; }
    }
    
    public class PostVacancyUserData
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("ukprn")]
        public int Ukprn { get; set; }
    }
    
    public class PostVacancyAddressData
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

    public class PostCreateVacancyWageData
    {
        [JsonPropertyName("wageAdditionalInformation")]
        public string WageAdditionalInformation { get ; set ; }
        [JsonPropertyName("fixedWageYearlyAmount")]
        public decimal? FixedWageYearlyAmount { get ; set ; }
        [JsonPropertyName("weeklyHours")]
        public decimal WeeklyHours { get ; set ; }
        [JsonPropertyName("duration")]
        public int Duration { get ; set ; }
        [JsonPropertyName("workingWeekDescription")]
        public string WorkingWeekDescription { get ; set ; }
        [JsonPropertyName("wageType")]
        public WageType WageType { get; set; }
        [JsonPropertyName("durationUnit")]
        public DurationUnit DurationUnit { get; set; }
    }

    public class PostCreateVacancyQualificationData
    {
        [JsonPropertyName("qualificationType")]
        public string QualificationType { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        [JsonPropertyName("grade")]
        public string Grade { get; set; }
        [JsonPropertyName("weighting")]
        public QualificationWeighting Weighting { get; set; }
    }

    public class ContactDetails
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
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
    public enum OwnerType
    {
        Employer,
        Provider
    }
}