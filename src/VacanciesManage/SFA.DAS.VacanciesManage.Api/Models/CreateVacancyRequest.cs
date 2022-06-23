using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.Api.Models
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
        /// The Training Provider and Account Legal Entity where the vacancy is. If creating as a training provider the UKPRN will not be editable.
        /// </summary>
        [JsonProperty("contractingParties", Required = Required.Always)]
        public ContractingParties ContractingParties { get ; set ; }

        /// <summary>
        /// The name of the vacancy or job role being advertised. Must contain the word apprentice or apprenticeship and be less than 100 characters.
        /// </summary>
        /// <example>Apprenticeship in Advanced Baking</example>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get ; set ; }

        /// <summary>
        /// What activities and duties will the apprentice be undertaking during the apprenticeship. Must not exceed 4000 characters
        /// </summary>
        [JsonProperty("description", Required = Required.Always)]
        public string Description { get ; set ; }

        /// <summary>
        /// The LARS code associated with the Standard you wish to create the advert for. This can be found from `GET referencedata/courses`
        /// </summary>
        /// <example>119</example>
        [JsonProperty("standardLarsCode", Required = Required.Always)]
        public string ProgrammeId { get ; set ; }
        /// <summary>
        /// If `EmployerNameOption` is set to `TradingName` then will be used and Trading name in the application updated. If it is set to `Anonymous` then will be used for the anonymous employer name. If it is set to `RegisteredName` then the Account Legal Entity name will be used. Must not exceed 100 characters
        /// </summary>
        [JsonProperty("alternativeEmployerName")]
        public string EmployerName { get ; set ; }
        /// <summary>
        /// A short description of the apprenticeship. Must not exceed 350 characters
        /// </summary>
        [JsonProperty("shortDescription", Required = Required.Always)]
        public string ShortDescription { get ; set ; }
        /// <summary>
        /// The number of apprentices that will be recruited into the role. Add as a numerical value, must be at least 1.
        /// </summary>
        [JsonProperty("numberOfPositions", Required = Required.Always)]
        public int NumberOfPositions { get ; set ; }
        /// <summary>
        /// What an apprentice can expect in terms of career progression after the apprenticeship ends. You may want to mention specific routes they could take once qualified. Must not exceed 4000 characters
        /// </summary>
        [JsonProperty("outcomeDescription", Required = Required.Always)]
        public string OutcomeDescription { get ; set ; }
        /// <summary>
        /// The last date for receiving new applications. This must be before  the start date of the apprenticeship. Must be a valid date. Closing date for applications cannot be today or earlier.
        /// </summary>
        [JsonProperty("closingDate", Required = Required.Always)]
        public DateTime ClosingDate { get ; set ; }
        /// <summary>
        /// The planned start date of the apprenticeship. This must be after the closing date. Must be a valid date. Possible apprenticeship start date can't be today or earlier. We advise using a date more than two weeks from now.
        /// </summary>
        [JsonProperty("startDate", Required = Required.Always)]
        public DateTime StartDate { get ; set ; }
        /// <summary>
        /// A brief description about the employer. Must not exceed 4000 characters 
        /// </summary>
        [JsonProperty("employerDescription", Required = Required.Always)]
        public string EmployerDescription { get ; set ; }
        /// <summary>
        /// The training the apprentice will undertake and the qualification they will get at the end of the apprenticeship. Add any certifications and levels of qualifications. Must not exceed 4000 characters. 
        /// </summary>
        [JsonProperty("trainingDescription", Required = Required.Always)]
        public string TrainingDescription { get ; set ; }
        /// <summary>
        /// Where the apprenticeship will be based, this could be a different location to the organisation address. Use the place the apprentice will spend most of their time.
        /// </summary>
        [JsonProperty("address")]
        public CreateVacancyAddress Address { get; set; }
        [JsonProperty("wage", Required = Required.Always)]
        public CreateVacancyWage Wage { get; set; }
        /// <summary>
        /// Add the desired skills and personal qualities you’d like the applicant to have in order for you to consider them. There is a selection available from `GET referencedata/skills` or you can add your own. You must include at least one desired skill
        /// </summary>
        [JsonProperty("skills", Required = Required.Always)]
        public List<string> Skills { get ; set ; }
        /// <summary>
        /// Select if you do not wish your company name to be listed on the advert. This could mean fewer people view your advert.
        /// </summary>
        [JsonProperty("employerNameOption")]
        public EmployerNameOption EmployerNameOption { get ; set ; }
        /// <summary>
        /// Provide the reason why the organisation would like to remain anonymous if chosen for <see cref="EmployerNameOption"/>. The reason must not be more than 4000 characters
        /// </summary>
        [JsonProperty("anonymousReason")]
        public string AnonymousReason { get ; set ; }
        /// <summary>
        /// Qualifications obtained from `GET referendata/qualifications`. You must supply at least one qualification required.
        /// </summary>
        [JsonProperty("qualifications", Required = Required.Always)]
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
        /// Select how the applications will be managed. This is either through Find an apprenticeship or an external site. If external `ApplicationUrl` must be set 
        /// </summary>
        [JsonProperty("applicationMethod", Required = Required.Always)]
        public CreateVacancyApplicationMethod ApplicationMethod { get ; set ; }
        /// <summary>
        /// Are you registered as a Disability Confident employer?
        /// </summary>
        [JsonProperty("disabilityConfident", Required = Required.Always)]
        public CreateVacancyDisabilityConfident DisabilityConfident { get ; set ; }
        /// <summary>
        /// Any other information the applicant should be aware of. Must not exceed 4000 characters 
        /// </summary>
        [JsonProperty("thingsToConsider")]
        public string ThingsToConsider { get ; set ; }
        /// <summary>
        /// You can choose to display website for your organisation. Website address must be a valid URL.
        /// </summary>
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
    
    
    /// <summary>
    /// Address for the apprenticeship advert. Must contain address line 1 and a valid postcode.
    /// </summary>
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
        /// <summary>
        /// Address line 1
        /// </summary>
        [JsonProperty("addressLine1")]
        [Required]
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Address line 2
        /// </summary>
        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Address line 3
        /// </summary>
        [JsonProperty("addressLine3")]
        public string AddressLine3 { get; set; }
        /// <summary>
        /// Address line 4
        /// </summary>
        [JsonProperty("addressLine4")]
        public string AddressLine4 { get; set; }
        /// <summary>
        /// Postcode
        /// </summary>
        [JsonProperty("postcode")]
        [Required]
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
                FixedWageYearlyAmount = source.WageType == WageType.FixedWage ?  source.FixedWageYearlyAmount : null,
                DurationUnit = (InnerApi.Requests.DurationUnit)durationUnit,
                WageType = (InnerApi.Requests.WageType)wageType
            };
        }
        /// <summary>
        /// Extra information about pay. 250 character limit
        /// </summary>
        [JsonProperty("wageAdditionalInformation")]
        public string WageAdditionalInformation { get ; set ; }
        /// <summary>
        /// If `WageType.FixedWage` then enter the yearly amount.
        /// </summary>
        [JsonProperty("fixedWageYearlyAmount")]
        public decimal? FixedWageYearlyAmount { get ; set ; }
        /// <summary>
        /// The total number of hours per week. This must include the 20% of time the apprentice will spend training, which could be offsite. Needs to be greater than 16 and less than 48.
        /// </summary>
        [JsonProperty("weeklyHours", Required = Required.Always)]
        public decimal WeeklyHours { get ; set ; }
        /// <summary>
        /// Expected duration must be at least 12 months. The minimum duration of each apprenticeship is based on the apprentice working at least 30 hours a week, including any off-the-job training they undertake.
        /// Extend the minimum duration when the working week is fewer than 30 hours using the following formula:
        /// 12 x 30/average weekly hours = new minimum duration in months; or 52 x 30/average weekly hours = new minimum duration in weeks
        /// </summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get ; set ; }
        /// <summary>
        /// A short description of the pattern of working hours over the week. Start time, end time and working days. You have up to 250 characters
        /// </summary>
        [JsonProperty("workingWeekDescription", Required = Required.Always)]
        public string WorkingWeekDescription { get ; set ; }
        [JsonProperty("wageType", Required = Required.Always)]
        public WageType WageType { get; set; }
        /// <summary>
        /// Used with <see cref="Duration"/> for duration in months or years
        /// </summary>
        [JsonProperty("durationUnit", Required = Required.Always)]
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
        /// Qualification Type can be obtained from `GET referencedata/qualifications`
        /// </summary>
        [JsonProperty("qualificationType")]
        public string QualificationType { get; set; }
        /// <summary>
        /// Add a subject you would like the applicant to have.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }
        /// <summary>
        /// Enter the grade. GCSEs must be in number format 1-9
        /// </summary>
        [JsonProperty("grade")]
        public string Grade { get; set; }
        /// <summary>
        /// Is the qualification essential or desirable
        /// </summary>
        [JsonProperty("weighting")]
        public QualificationWeighting Weighting { get; set; }
    }

    public class SubmitterContactDetails
    {
        /// <summary>
        /// The name of the person creating the advert
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The email address of the person creating the advert
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The phone number of the person creating the advert
        /// </summary>
        public string Phone { get; set; }
    }

    public class ContractingParties
    {
        /// <summary>
        /// The UKPRN of the training provider you will be working with for this apprenticeship.
        /// </summary>
        [JsonProperty("ukprn")]
        public int Ukprn { get; set; }
        /// <summary>
        /// The Account Legal Entity public hashed Id of the organisation that you wish to create the vacancy for. This can be obtained from `GET accountlegalentites`
        /// </summary>
        [Required]
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
    
    /// <summary>
    /// Choose from:
    /// `WageType.FixedWage` This must be more than the [National Minimum Wage for apprentices](https://www.gov.uk/national-minimum-wage-rates). From 1 April 2021, the National Minimum Wage for apprentices is £4.30 an hour. Based on 16 working hours a week, you'll need to pay a yearly wage of at least £3,577.60.
    /// `WageType.NationalMinimumWageForApprentices` From 1 April 2021, the National Minimum Wage for apprentices is £4.30 an hour. On the advert, this will be displayed as a yearly wage of £3,577.60.
    /// `WageType.NationalMinimumWage` From 1 April 2021, the National Minimum Wage is between £4.62 and £8.91 an hour, depending on the candidate's age. On the advert, this will be displayed as a yearly wage of £3,843.84 to £7,413.12.
    /// </summary>
    public enum WageType
    {
        FixedWage,
        NationalMinimumWageForApprentices,
        NationalMinimumWage
    }
    
    /// <summary>
    /// Used in combination with `Wage.Duration` to specify the length of the apprenticeship. 
    /// </summary>
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