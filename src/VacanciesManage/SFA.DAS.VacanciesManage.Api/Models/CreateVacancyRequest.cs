using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

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

            var locationOption = source.RecruitingNationally
                ? AvailableWhere.AcrossEngland
                : source.MultipleAddresses is { Count: > 0 }
                    ? AvailableWhere.MultipleLocations
                    : AvailableWhere.OneLocation;

            List<PostVacancyAddressData> addresses = null;
            switch (locationOption)
            {
                case AvailableWhere.OneLocation:
                    addresses = source.Address is null
                        ? null
                        : [(PostVacancyAddressData)source.Address];
                    break;
                case AvailableWhere.MultipleLocations:
                    addresses = source.MultipleAddresses.Select(x => (PostVacancyAddressData)x).ToList();
                    break;
            }
            
            return new PostVacancyRequestData
            {
                AccountLegalEntityPublicHashedId = source.ContractingParties.AccountLegalEntityPublicHashedId,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                Addresses = addresses,
                AnonymousReason = source.AnonymousReason,
                ApplicationInstructions = source.ApplicationInstructions,
                ApplicationMethod = (InnerApi.Requests.CreateVacancyApplicationMethod)applicationMethod,
                ApplicationUrl = source.ApplicationUrl,
                ClosingDate = source.ClosingDate,
                Description = source.Description,
                DisabilityConfident = (InnerApi.Requests.CreateVacancyDisabilityConfident)disabilityConfident,
                EmployerDescription = source.EmployerDescription,
                EmployerLocationInformation = source.RecruitingNationallyDetails,
                EmployerLocationOption = locationOption,
                EmployerName = source.EmployerName,
                EmployerNameOption = (InnerApi.Requests.EmployerNameOption)employerNameOption,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                NumberOfPositions = source.NumberOfPositions,
                OutcomeDescription = source.OutcomeDescription,
                ProgrammeId = source.ProgrammeId,
                Qualifications = source.Qualifications != null ? source.Qualifications.Select(c => (PostCreateVacancyQualificationData)c).ToList() : [],
                ShortDescription = source.ShortDescription,
                Skills = source.Skills != null ? source.Skills.ToList() : [],
                StartDate = source.StartDate,
                ThingsToConsider = source.ThingsToConsider,
                Title = source.Title,
                TrainingDescription = source.TrainingDescription,
                User = Map(source.SubmitterContactDetails, source.ContractingParties),
                Wage = source.Wage,
            };
        }

        /// <summary>
        /// The title for the apprenticeship vacancy on Find an apprenticeship. Must include: <b>apprentice</b> or <b>apprenticeship</b>.
        /// </summary>
        /// <example>Library assistant apprenticeship</example>
        [JsonPropertyName("title")]
        [Required]
        [MaxLength(100)]
        public string Title { get ; set ; }

        /// <summary>
        /// The number of apprentices being recruited for the apprenticeship.
        /// </summary>
        [JsonPropertyName("numberOfPositions")]
        [Required]
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// The last date people can apply for the apprenticeship. Must be at least 2 weeks in the future.
        /// </summary>
        /// <example>2019-08-24T:14:15:22Z</example>
        [JsonPropertyName("closingDate")]
        [Required]
        public DateTime ClosingDate { get; set; }

        /// <summary>
        /// The planned date for the apprenticeship’s start. We suggest using a date 2 weeks after the closing date.
        /// </summary>
        /// <example>2019-08-24T:14:15:22Z</example>
        [JsonPropertyName("startDate")]
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The Training Provider and Account Legal Entity where the vacancy is. If creating as a training provider the UKPRN will not be editable.
        /// </summary>
        [JsonPropertyName("contractingParties")]
        [Required]
        public ContractingParties ContractingParties { get ; set ; }

        [JsonPropertyName("wage")]
        [Required]
        public CreateVacancyWage Wage { get; set; }
        
        /// <summary>
        /// Where the apprenticeship will be based, this could be a different location to the organisation address. Use the place the apprentice will spend most of their time. Must be within England.
        /// </summary>
        [JsonPropertyName("address")]
        public CreateVacancyAddress Address { get; set; }
        
        /// <summary>
        /// If the apprenticeship is available at more than one location, use this to submit multiple addresses for the vacancy. You can submit up to and including 10 addresses. Each location must contain address line 1 and a valid postcode. Each location must be within England.
        /// </summary>
        [JsonPropertyName("multipleAddresses")]
        [MinLength(2)]
        [MaxLength(10)]
        public List<CreateVacancyAddress> MultipleAddresses { get; set; }

        /// <summary>
        /// If the apprenticeship is available to applicants across the entirety of England, you can advertise it nationally. For example, if your apprenticeship is available in many locations across England, remote working or provides live-in accommodation. The vacancy will display on Find an apprenticeship for searches across England. When recruitingNationally is true,  you cannot use address or multipleAddresses. If true, you must also include recruitingNationallyDetails. 
        /// </summary>
        [JsonPropertyName("recruitingNationally")]
        public bool RecruitingNationally { get; set; }
        
        /// <summary>
        /// Explain why recruitingNationally is true by giving more information to applicants about where they will work. Required if recruitingNationally is true.
        /// </summary>
        [JsonPropertyName("recruitingNationallyDetails")]
        [MaxLength(500)]
        public string RecruitingNationallyDetails { get; set; }

        /// <summary>
        /// A short summary of the overall apprenticeship. This appears at the top of the vacancy on Find an apprenticeship.
        /// </summary>
        [JsonPropertyName("shortDescription")]
        [MaxLength(350)]
        [Required]
        public string ShortDescription { get ; set ; }
        /// <summary>
        /// What the apprentice will do at work. We suggest including day-to-day duties. Must include at least 3 bullet points (written in HTML).
        /// </summary>
        /// <example>Your daily tasks could include: <ul><li> working within a team to deliver a menu of high-quality food each service</li> <li> prepare food in a way that meets food hygiene standards</li> <li> storing food correctly</li> </ul></example>
        [JsonPropertyName("description")]
        [MaxLength(4000)]
        [Required]
        public string Description { get ; set ; }
        /// <summary>
        /// What progression or outcome the apprentice can expect at the end of the apprenticeship.
        /// </summary>
        [JsonPropertyName("outcomeDescription")]
        [MaxLength(4000)]
        [Required]
        public string OutcomeDescription { get ; set ; }
        /// <summary>
        /// Where and when an apprentice’s training will take place. Don’t include any other information about the training here.
        /// If you submit an advert without providing this field, we’ll display a message on Find an apprenticeship saying that the training schedule hasn’t been agreed yet.
        /// </summary>
        /// <example>One day per week, in-person at college</example>
        [JsonPropertyName("trainingDescription")]
        [MaxLength(4000)]
        [Required]
        public string TrainingDescription { get ; set ; }
        /// <summary>
        /// Further information about an apprentice’s training, such as details about the training provider or how the course will be structured.
        /// </summary>
        /// <example>You’ll have an apprenticeship advisor who will support you through your apprenticeship.</example>
        [JsonPropertyName("additionalTrainingDescription")]
        [MaxLength(4000)]
        public string AdditionalTrainingDescription { get; set; }
        /// <summary>
        /// The code from the learning aim reference service (LARS) for the apprenticeship’s training course. 
        /// If the LARS code is for a foundation apprenticeship, you cannot submit any `qualifications` or `skills` as a foundation apprenticeship cannot have these application requirements. 
        /// See all codes using `GET referencedata/courses`.
        /// </summary>
        /// <example>119</example>
        [JsonPropertyName("standardLarsCode")]
        [Required]
        public string ProgrammeId { get ; set ; }
        /// <summary>
        /// Select if you do not wish your company name to be listed on the advert. This could mean fewer people view your advert.
        /// </summary>
        [JsonPropertyName("employerNameOption")]
        public EmployerNameOption EmployerNameOption { get ; set ; }
        /// <summary>
        /// When `employerNameOption` is set to `tradingName`, use this field to set the company’s name yourself with the correct formatting.
        /// </summary>
        [JsonPropertyName("alternativeEmployerName")]
        [MaxLength(100)]
        public string EmployerName { get ; set ; }
        /// <summary>
        /// When `employerNameOption` is set to `anonymousName`, give a brief description of the company to help people understand what they do.
        /// </summary>
        /// <example>Car manufacturer or clothes retailer</example>
        [JsonPropertyName("employerDescription")]
        [Required]
        public string EmployerDescription { get ; set ; }
        /// <summary>
        /// When `employerNameOption` is set to `anonymousName`, tell us why you need to hide the company's name.
        /// This will not appear on Find an apprenticeship but is needed for our quality assurance team to approve your vacancy.
        /// </summary>
        [JsonPropertyName("anonymousReason")]
        public string AnonymousReason { get ; set ; }
        /// <summary>
        /// The web address for the employer’s website.
        /// </summary>
        [JsonPropertyName("employerWebsiteUrl")]
        public string EmployerWebsiteUrl { get; set; }
        /// <summary>
        /// Are you registered as a Disability Confident employer?
        /// </summary>
        [JsonPropertyName("disabilityConfident")]
        [Required]
        public CreateVacancyDisabilityConfident DisabilityConfident { get ; set ; }

        [JsonPropertyName("submitterContactDetails")]
        [Required]
        public SubmitterContactDetails SubmitterContactDetails { get ; set ; }

        /// <summary>
        /// Skills and qualities an apprentice should have for this apprenticeship. We’ll show this on the vacancy.
        /// If `applicationMethod` is `ThroughFindAnApprenticeship`, we’ll also ask applicants for examples of when they’ve used these skills.
        /// Use `GET referencedata/skills` to see our default selection of skills or add your own.
        /// If `standardsLarsCode` is for a foundation apprenticeship, you cannot submit any data for this field. This is because foundations cannot have application requirements.
        /// </summary>
        [JsonPropertyName("skills")]
        public List<string> Skills { get ; set ; }
        /// <summary>
        /// Qualifications obtained from `GET referendata/qualifications`. 
        /// If standardsLarsCode is for a foundation apprenticeship, you cannot submit any data for this field. 
        /// This is because foundations cannot have application requirements. Otherwise, you must supply at least one qualification required.
        /// </summary>
        [JsonPropertyName("qualifications")]
        public List<CreateVacancyQualification> Qualifications { get; set; }
        /// <summary>
        /// Other requirements for the applicant, such as needing a Disclosure and Barring Service (DBS) check.
        /// </summary>
        [JsonPropertyName("thingsToConsider")]
        [MaxLength(4000)]
        public string ThingsToConsider { get ; set ; }
        /// <summary>
        /// Select how the applications will be managed. This is either through Find an apprenticeship or an external site. If external `ApplicationUrl` must be set 
        /// </summary>
        [JsonPropertyName("applicationMethod")]
        [Required]
        public CreateVacancyApplicationMethod ApplicationMethod { get ; set ; }
        /// <summary>
        /// If `applicationMethod` is `throughExternalSite`, you can give some information about the application process.
        /// </summary>
        [JsonPropertyName("applicationInstructions")]
        public string ApplicationInstructions { get ; set ; }
        /// <summary>
        /// If `applicationMethod` is `throughExternalSite`, provide the web address for your application website.
        /// </summary>
        [JsonPropertyName("applicationUrl")]
        public string ApplicationUrl { get ; set ; }
        /// <summary>
        /// If `applicationMethod` is `throughFindAnApprenticeship`, you can add questions for us to add to the application form.
        /// Note that we automatically ask all applicants ‘What are your skills and strengths?’ and ‘What interests you about this apprenticeship?’
        /// </summary>
        /// <example>Do you have a driving licence?</example>
        [JsonPropertyName("additionalQuestion1")]
        [MaxLength(250)]
        [RegularExpression(@"^\w(.*)\w\?(\s\w(.*)\w[\.\?])*$", ErrorMessage = "Must include '?'")]
        public string AdditionalQuestion1 { get; set; }
        /// <summary>
        /// If `applicationMethod` is `throughFindAnApprenticeship`, add another question to the application form.
        /// </summary>
        /// <example>What is your interest in this industry?</example>
        [JsonPropertyName("additionalQuestion2")]
        [MaxLength(250)]
        [RegularExpression(@"^\w(.*)\w\?(\s\w(.*)\w[\.\?])*$", ErrorMessage = "Must include '?'")]
        public string AdditionalQuestion2 { get; set; }


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
    /// Address for the apprenticeship advert. Must contain address line 1 and a valid postcode. Must be within England.
    /// </summary>
    public class CreateVacancyAddress
    {
        public static implicit operator PostVacancyAddressData(CreateVacancyAddress source)
        {
            return source is null
                ? null
                : new PostVacancyAddressData
                {
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    AddressLine3 = source.AddressLine3,
                    AddressLine4 = source.AddressLine4,
                    Postcode = source.Postcode
                };
        }
        /// <summary>
        /// First line of the address where the apprentice will work.
        /// </summary>
        [JsonPropertyName("addressLine1")]
        [Required]
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Second line of the address where the apprentice will work.
        /// </summary>
        [JsonPropertyName("addressLine2")]
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Third line of the address where the apprentice will work.
        /// </summary>
        [JsonPropertyName("addressLine3")]
        public string AddressLine3 { get; set; }
        /// <summary>
        /// Fourth line of the address where the apprentice will work.
        /// </summary>
        [JsonPropertyName("addressLine4")]
        public string AddressLine4 { get; set; }
        /// <summary>
        /// Postcode of the address where the apprentice will work.
        /// </summary>
        [JsonPropertyName("postcode")]
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
                WageType = (InnerApi.Requests.WageType)wageType,
                CompanyBenefitsInformation = source.CompanyBenefitsInformation,
            };
        }

        [JsonPropertyName("wageType")]
        [Required]
        public WageType WageType { get; set; }

        /// <summary>
        /// If `WageType` is `FixedWage`, enter the yearly amount the apprentice will earn.
        /// </summary>
        /// <example>25000</example>
        [JsonPropertyName("fixedWageYearlyAmount")]
        public decimal? FixedWageYearlyAmount { get ; set ; }
        /// <summary>
        /// Additional information about pay, such as when the apprentice might get a pay rise.
        /// </summary>
        [JsonPropertyName("wageAdditionalInformation")]
        [MaxLength(250)]
        public string WageAdditionalInformation { get ; set ; }
        /// <summary>
        /// Describe benefits the company offers.
        /// </summary>
        [JsonPropertyName("CompanyBenefitsInformation")]
        [MaxLength(250)]
        public string CompanyBenefitsInformation { get; set; }
        /// <summary>
        /// The total number of hours per week. This includes both work and training. Needs to be between 16 and 48 hours.
        /// </summary>
        [JsonPropertyName("weeklyHours")]
        [Required]
        public decimal WeeklyHours { get ; set ; }
        /// <summary>
        /// Information about the working schedule, such as daily working hours.
        /// </summary>
        [JsonPropertyName("workingWeekDescription")]
        [MaxLength(250)]
        [Required]
        public string WorkingWeekDescription { get ; set ; }
        /// <summary>
        /// How long the apprenticeship will be. Use duration to set a number, and then `durationUnit` to say whether it’s months or years. Apprenticeships must be longer than a year.
        /// </summary>
        /// <example>18</example>
        [JsonPropertyName("duration")]
        [Required]
        public int Duration { get ; set ; }

        /// <summary>
        /// Used with <see cref="Duration"/> for duration in months or years
        /// </summary>
        [JsonPropertyName("durationUnit")]
        [Required]
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
                Weighting = (InnerApi.Requests.QualificationWeighting)weighting,
                Level = source.Level,
            };
        }
        /// <summary>
        /// The type of qualification you want. Use `GET referencedata/qualifications` to see what qualification types you can use.
        /// </summary>
        [JsonPropertyName("qualificationType")]
        public string QualificationType { get; set; }
        /// <summary>
        /// If `qualificationType` is `BTEC`, you must tell us what level of BTEC you’re looking for.
        /// </summary>
        /// <example>7</example>
        [JsonPropertyName("level")]
        [AllowedValues(null,1,2,3,4,5,6,7)]
        public int? Level { get; set; }
        /// <summary>
        /// The name of the subject for the qualification.
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        /// <summary>
        /// The grade for the qualification. GCSEs must include the 1 to 9 grading system.
        /// </summary>
        /// <example>C or 4</example>
        [JsonPropertyName("grade")]
        public string Grade { get; set; }
        /// <summary>
        /// Is the qualification essential or desirable
        /// </summary>
        [JsonPropertyName("weighting")]
        public QualificationWeighting Weighting { get; set; }
    }

    public class SubmitterContactDetails
    {
        /// <summary>
        /// The name of a contact who applicants can contact to discuss the apprenticeship.
        /// </summary>
        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// The contact's email address.
        /// </summary>
        [JsonPropertyName("email")]
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// The contact's phone number.
        /// </summary>
        [JsonPropertyName("phone")]
        [Required]
        public string Phone { get; set; }
    }

    public class ContractingParties
    {
        /// <summary>
        /// The unique identification number for the apprentice’s employer. Use `GET accountlegalentites` to obtain the hashed ID for the employer.
        /// </summary>
        [Required]
        public string AccountLegalEntityPublicHashedId { get; set; }
        /// <summary>
        /// The UK provider reference number (UKPRN) for the apprenticeship’s training provider.
        /// </summary>
        [JsonPropertyName("ukprn")]
        public int Ukprn { get; set; }
    }

    /// <summary>
    /// How much you want an application to have the qualification.
    /// </summary>
    public enum QualificationWeighting
    {
        Essential,
        Desired
    }
    /// <summary>
    /// Choose from:
    /// <ul><li>`throughFindAnApprenticeship` lets applicants apply through Find an apprenticeship.</li>
    /// <li>`throughExternalApplicationSite` means you’ll use your own website to accept applications.</li></ul>
    /// </summary>
    public enum CreateVacancyApplicationMethod
    {
        ThroughFindAnApprenticeship,
        ThroughExternalApplicationSite
    }

    /// <summary>
    /// Choose from:
    /// `WageType.NationalMinimumWageForApprentices` sets the wage to the National Minimum Wage for apprentices
    /// `WageType.NationalMinimumWage` sets the wage to the National Minimum Wage 
    /// `WageType.FixedWage` lets you set a fixed wage for the apprenticeship
    /// `WageType.CompetitiveSalary` does not set an exact wage and shows the word ‘Competitive’
    /// National Minimum Wages will change every year on 1 April. We automatically update adverts to the correct National Minimum Wages.
    /// </summary>
    /// <example>fixedWage</example>
    public enum WageType
    {
        FixedWage,
        NationalMinimumWageForApprentices,
        NationalMinimumWage,
        CompetitiveSalary
    }
    
    /// <summary>
    /// Set the unit of time for `duration`. 
    /// </summary>
    public enum DurationUnit
    {
        Month,
        Year
    }
    /// <summary>
    /// Choose how the employer’s company name appears on Find an apprenticeship. Choose from: 
    /// `registeredName` uses company’s name from the Companies House database (normally in all caps).
    /// `tradingName` lets you set the company’s name yourself, using `alternativeEmployerName` to correctly format the name.
    /// `anonymous` hides the company’s name on the vacancy. Note you’ll need to provide `employerDescription` and `anonymousReason`
    /// </summary>
    public enum EmployerNameOption
    {
        RegisteredName,
        TradingName,
        Anonymous
    }
    /// <summary>
    /// Say whether the employer is part of the Department for Work and Pension’s Disability Confident scheme. If Yes:
    /// <ul><li>we’ll show a Disability Confident logo on the vacancy</li>
    /// <li>If `applicationMethod` is `throughFindAnApprenticeship`, we'll ask applicants whether they want to apply through the Disability Confident scheme.</li></ul>
    /// </summary>
    public enum CreateVacancyDisabilityConfident
    {
        No = 0,
        Yes
    }
}