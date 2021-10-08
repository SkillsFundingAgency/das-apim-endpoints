using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Api.Models
{
    public class CreateVacancyRequest
    {
        public static implicit operator PostVacancyRequestData(CreateVacancyRequest source)
        {
            Enum.TryParse(typeof(InnerApi.Requests.EmployerNameOption), source.EmployerNameOption.ToString(), true,
                out var employerNameOption);
            Enum.TryParse(typeof(InnerApi.Requests.CreateVacancyApplicationMethod), source.ApplicationMethod.ToString(), true,
                out var applicationMethod);
            Enum.TryParse(typeof(InnerApi.Requests.CreateVacancyDisabilityConfident), source.DisabilityConfident.ToString(), true,
                out var disabilityConfident);
            
            return new PostVacancyRequestData
            {
                Title = source.Title,
                Description = source.Description,
                ProgrammeId = source.ProgrammeId,
                EmployerAccountId = source.EmployerAccountId,
                User = source.User,
                EmployerName = source.EmployerName,
                ShortDescription = source.ShortDescription,
                NumberOfPositions = source.NumberOfPositions,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                ClosingDate = source.ClosingDate,
                StartDate = source.StartDate,
                LegalEntityName = source.LegalEntityName,
                EmployerDescription = source.EmployerDescription,
                TrainingDescription = source.TrainingDescription,
                Address = source.Address,
                Wage = source.Wage,
                Skills = source.Skills,
                EmployerNameOption = (InnerApi.Requests.EmployerNameOption)employerNameOption,
                AnonymousReason = source.AnonymousReason,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationUrl = source.ApplicationUrl,
                ThingsToConsider = source.ThingsToConsider,
                Qualifications = source.Qualifications.Select(c=>(PostCreateVacancyQualificationData)c).ToList(),
                ApplicationMethod = (InnerApi.Requests.CreateVacancyApplicationMethod)applicationMethod,
                DisabilityConfident = (InnerApi.Requests.CreateVacancyDisabilityConfident)disabilityConfident
            };
        }
        
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
        // [JsonProperty("outcomeDescription")]//TODO check this is required
        // public string OutcomeDescription { get ; set ; }
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
        public static implicit operator PostVacancyUserData(VacancyUser source)
        {
            return new PostVacancyUserData
            {
                Email = source.Email,
                Name = source.Name,
                Ukprn = source.Ukprn,
                UserId = source.UserId
            };
        }
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
        public static implicit operator PostVacancyAddressData(CreateVacancyAddress source)
        {
            return new PostVacancyAddressData
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode
            };
        }
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
        public static implicit operator PostCreateVacancyWageData(CreateVacancyWage source)
        {
            Enum.TryParse(typeof(InnerApi.Requests.WageType), source.WageType.ToString(), true,
                out var wageType);
            Enum.TryParse(typeof(InnerApi.Requests.DurationUnit), source.DurationUnit.ToString(), true,
                out var durationUnit);
            return new PostCreateVacancyWageData
            {
                WageAdditionalInformation = source.WageAdditionalInformation,
                WeeklyHours = source.WeeklyHours,
                Duration = source.Duration,
                WorkingWeekDescription = source.WorkingWeekDescription,
                FixedWageYearlyAmount = source.FixedWageYearlyAmount,
                DurationUnit = (InnerApi.Requests.DurationUnit)durationUnit,
                WageType = (InnerApi.Requests.WageType)wageType
            };
        }
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
        public static implicit operator PostCreateVacancyQualificationData(CreateVacancyQualification source)
        {
            Enum.TryParse(typeof(InnerApi.Requests.QualificationWeighting), source.Weighting.ToString(), true,
                out var weighting);
            return new PostCreateVacancyQualificationData
            {
                QualificationType = source.QualificationType,
                Grade = source.Grade,
                Subject = source.Subject,
                Weighting = (InnerApi.Requests.QualificationWeighting)weighting
            };
        }
        
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