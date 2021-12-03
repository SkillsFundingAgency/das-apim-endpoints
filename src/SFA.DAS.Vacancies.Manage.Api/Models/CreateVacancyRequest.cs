using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
                User = Map(source.SubmitterContactDetails, source.ContractingParties),
                EmployerName = source.EmployerName,
                ShortDescription = source.ShortDescription,
                NumberOfPositions = source.NumberOfPositions,
                AccountLegalEntityPublicHashedId = source.ContractingParties.AccountLegalEntityPublicHashedId,
                ClosingDate = source.ClosingDate,
                StartDate = source.StartDate,
                EmployerDescription = source.EmployerDescription,
                TrainingDescription = source.TrainingDescription,
                Address = source.Address,
                Wage = source.Wage,
                Skills = source.Skills,
                OutcomeDescription = source.OutcomeDescription,
                EmployerNameOption = (InnerApi.Requests.EmployerNameOption)employerNameOption,
                AnonymousReason = source.AnonymousReason,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationUrl = source.ApplicationUrl,
                ThingsToConsider = source.ThingsToConsider,
                Qualifications = source.Qualifications.Select(c=>(PostCreateVacancyQualificationData)c).ToList(),
                ApplicationMethod = (InnerApi.Requests.CreateVacancyApplicationMethod)applicationMethod,
                DisabilityConfident = (InnerApi.Requests.CreateVacancyDisabilityConfident)disabilityConfident,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl
            };
        }

        /// <summary>
        /// Contact details to be used if the ESFA needs to get in touch about your advert.
        /// </summary>
        [JsonProperty("submitterContactDetails", Required = Required.Always)]
        public SubmitterContactDetails SubmitterContactDetails { get ; set ; }

        /// <summary>
        /// The training provider and Account Legal Entity where the vacancy is. If creating as a training provider the UKPRN will not be editable.
        /// </summary>
        [JsonProperty("contractingParties", Required = Required.Always)]
        public ContractingParties ContractingParties { get ; set ; }

        /// <summary>
        /// The name of the vacancy or job role being advertised.
        /// </summary>
        /// <example>Apprenticeship in Advanced Baking</example>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get ; set ; }

        /// <summary>
        /// What activities and duties will the apprentice be undertaking during the apprenticeship. 
        /// </summary>
        [JsonProperty("description", Required = Required.Always)]
        public string Description { get ; set ; }

        /// <summary>
        /// The Id of Apprenticeship standard. This can be obtained from the GET referenceData/courses endpoint.
        /// </summary>
        /// <example>119</example>
        [JsonProperty("standardLarsCode")]
        public string ProgrammeId { get ; set ; }
        /// <summary>
        /// Name of the organisation
        /// </summary>
        [JsonProperty("employerName")]
        public string EmployerName { get ; set ; }
        /// <summary>
        /// A short description of the apprenticeship.
        /// </summary>
        [JsonProperty("shortDescription")]
        public string ShortDescription { get ; set ; }
        /// <summary>
        /// How many apprentices will be recruited into the role.
        /// </summary>
        [JsonProperty("numberOfPositions")]
        public int NumberOfPositions { get ; set ; }
        /// <summary>
        /// What an apprentice can expect in terms of career progression after the apprenticeship ends.
        /// </summary>
        [JsonProperty("outcomeDescription")]
        public string OutcomeDescription { get ; set ; }
        /// <summary>
        /// The last date for receiving new applications.
        /// </summary>
        [JsonProperty("closingDate")]
        public DateTime ClosingDate { get ; set ; }
        /// <summary>
        /// The planned start date of the apprenticeship.
        /// </summary>
        [JsonProperty("startDate")]
        public DateTime StartDate { get ; set ; }
        /// <summary>
        /// A brief description about the employer.
        /// </summary>
        [JsonProperty("employerDescription")]
        public string EmployerDescription { get ; set ; }
        /// <summary>
        /// The training the apprentice will undertake and the qualification they will get at the end of the apprenticeship. Add any certifications and levels of qualifications.
        /// </summary>
        [JsonProperty("trainingDescription")]
        public string TrainingDescription { get ; set ; }
        /// <summary>
        /// Where the apprenticeship will be based, this could be a different location to the organisation address. Use the place the apprentice will spend most of their time.
        /// </summary>
        [JsonProperty("address")]
        public CreateVacancyAddress Address { get; set; }
        [JsonProperty("wage")]
        public CreateVacancyWage Wage { get; set; }
        /// <summary>
        /// Select the desired skills and personal qualities you’d like the applicant to have in order for you to consider them. This is available from GET referencedata/skills
        /// </summary>
        [JsonProperty("skills")]
        public List<string> Skills { get ; set ; }
        [JsonProperty("employerNameOption")]
        public EmployerNameOption EmployerNameOption { get ; set ; }
        /// <summary>
        /// Provide the reason why the organisation would like to remain anonymous.
        /// </summary>
        [JsonProperty("anonymousReason")]
        public string AnonymousReason { get ; set ; }
        [JsonProperty("qualifications")]
        public List<CreateVacancyQualification> Qualifications { get; set; }
        /// <summary>
        /// Information for applicants about how their applications will be managed externally.
        /// </summary>
        [JsonProperty("applicationInstructions")]
        public string ApplicationInstructions { get ; set ; }
        /// <summary>
        /// If they are being managed externally, add the web address of the employer/agency managing the applications.
        /// </summary>
        [JsonProperty("applicationUrl")]
        public string ApplicationUrl { get ; set ; }
        /// <summary>
        /// Select how the applications will be managed.
        /// </summary>
        [JsonProperty("applicationMethod")]
        public CreateVacancyApplicationMethod ApplicationMethod { get ; set ; }
        /// <summary>
        /// Are you registered as a Disability Confident employer?
        /// </summary>
        [JsonProperty("disabilityConfident")]
        public CreateVacancyDisabilityConfident DisabilityConfident { get ; set ; }
        /// <summary>
        /// Any other information the applicant should be aware of.
        /// </summary>
        [JsonProperty("thingsToConsider")]
        public string ThingsToConsider { get ; set ; }
        [JsonProperty("employerWebsiteUrl")]
        public string EmployerWebsiteUrl { get; set; }
        
        public static PostVacancyUserData Map(SubmitterContactDetails submitterContactDetails, ContractingParties contractingParties)
        {
            return new PostVacancyUserData
            {
                Email = submitterContactDetails.Email,
                Name = submitterContactDetails.Name,
                Ukprn = contractingParties.Ukprn
            };
        }
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
        public int Ukprn { get; set; }
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
        /// <summary>
        /// Qualification Type can be obtained from GET referencedata/qualifications
        /// </summary>
        [JsonProperty("qualificationType")]
        public string QualificationType { get; set; }
        /// <summary>
        /// Add a subject you would like the applicant to have.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
        /// <summary>
        /// Select a grade you would like the applicant to have.
        /// </summary>
        [JsonProperty("grade")]
        public string Grade { get; set; }
        /// <summary>
        /// Is the qualification essential or desirable? 
        /// </summary>
        [JsonProperty("weighting")]
        public QualificationWeighting Weighting { get; set; }
    }

    public class SubmitterContactDetails
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class ContractingParties
    {
        /// <summary>
        /// The UKPRN of the training provider you will be working with for this apprenticeship.
        /// </summary>
        public int Ukprn { get; set; }
        /// <summary>
        /// The Account Legal Entity Public Hashed Id of the employer for this apprenticeship. This can be obtained from GET AccountLegalEntities/
        /// </summary>
        public string AccountLegalEntityPublicHashedId { get; set; }
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