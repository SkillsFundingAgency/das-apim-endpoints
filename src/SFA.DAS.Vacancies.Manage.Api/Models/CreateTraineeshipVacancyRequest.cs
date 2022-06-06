using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Api.Models
{
    public class CreateTraineeshipVacancyRequest
    {
        public static implicit operator PostTraineeshipVacancyRequestData(CreateTraineeshipVacancyRequest source)
        {
            Enum.TryParse(typeof(InnerApi.Requests.EmployerNameOption), source.EmployerNameOption.ToString(), true,
                out var employerNameOption);
            Enum.TryParse(typeof(InnerApi.Requests.CreateVacancyDisabilityConfident), source.DisabilityConfident.ToString(), true,
                out var disabilityConfident);

            return new PostTraineeshipVacancyRequestData
            {
                Title = source.Title,
                Description = source.Description,
                RouteId = source.RouteId,
                LegalEntityName = source.LegalEntityName,
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
                Wage = source.DurationAndWorkingHours,
                Skills = source.Skills,
                OutcomeDescription = source.OutcomeDescription,
                EmployerNameOption = (InnerApi.Requests.TraineeshipEmployerNameOption)employerNameOption,
                AnonymousReason = source.AnonymousReason,
                DisabilityConfident = (InnerApi.Requests.CreateTraineeshipVacancyDisabilityConfident)disabilityConfident,
                WorkExperience = source.WorkExperience
            };
        }

        /// <summary>
        /// Contact details to be used if the ESFA needs to get in touch about your advert.
        /// </summary>
        [JsonProperty("submitterContactDetails", Required = Required.Always)]
        public TraineeshipSubmitterContactDetails SubmitterContactDetails { get; set; }

        /// <summary>
        /// The Training Provider and Account Legal Entity where the vacancy is. If creating as a training provider the UKPRN will not be editable.
        /// </summary>
        [JsonProperty("contractingParties", Required = Required.Always)]
        public TraineeshipContractingParties ContractingParties { get; set; }

        /// <summary>
        /// The name of the vacancy or job role being advertised. Must contain the word trainee or traineeship and be less than 100 characters.
        /// </summary>
        /// <example>Traineeship in Construction</example>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// What activities and duties will the trainee be undertaking during the traineeship. Must not exceed 4000 characters
        /// </summary>
        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }
        /// <summary>
        /// The route Id of the traineeship. Can be taken from `GET referencedata/routes`
        /// </summary>
        [JsonProperty("routeId", Required = Required.Always)]
        public int RouteId { get; set; }

        [JsonProperty("legalEntityName", Required = Required.Always)]
        public string LegalEntityName { get; set; }
        /// <summary>
        /// If `EmployerNameOption` is set to `TradingName` then will be used and Trading name in the application updated. If it is set to `Anonymous` then will be used for the anonymous employer name. If it is set to `RegisteredName` then the Account Legal Entity name will be used. Must not exceed 100 characters
        /// </summary>
        [JsonProperty("alternativeEmployerName")]
        public string EmployerName { get; set; }
        /// <summary>
        /// A short description of the traineeship. Must not exceed 350 characters
        /// </summary>
        [JsonProperty("shortDescription", Required = Required.Always)]
        public string ShortDescription { get; set; }
        /// <summary>
        /// The number of trainees that will be recruited into the role. Add as a numerical value, must be at least 1.
        /// </summary>
        [JsonProperty("numberOfPositions", Required = Required.Always)]
        public int NumberOfPositions { get; set; }
        /// <summary>
        /// What a trainee can expect in terms of career progression after the traineeship ends. You may want to mention specific routes they could take once qualified. Must not exceed 4000 characters
        /// </summary>
        [JsonProperty("outcomeDescription", Required = Required.Always)]
        public string OutcomeDescription { get; set; }
        /// <summary>
        /// The last date for receiving new applications. This must be before  the start date of the traineeship. Must be a valid date. Closing date for applications cannot be today or earlier.
        /// </summary>
        [JsonProperty("closingDate", Required = Required.Always)]
        public DateTime ClosingDate { get; set; }
        /// <summary>
        /// The planned start date of the traineeship. This must be after the closing date. Must be a valid date. Possible traineeship start date can't be today or earlier. We advise using a date more than two weeks from now.
        /// </summary>
        [JsonProperty("startDate", Required = Required.Always)]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// A brief description about the employer. Must not exceed 4000 characters 
        /// </summary>
        [JsonProperty("employerDescription", Required = Required.Always)]
        public string EmployerDescription { get; set; }
        /// <summary>
        /// The training the trainee will undertake and the qualification they will get at the end of the traineeship. Add any certifications and levels of qualifications. Must not exceed 4000 characters. 
        /// </summary>
        [JsonProperty("trainingDescription", Required = Required.Always)]
        public string TrainingDescription { get; set; }
        /// <summary>
        /// Where the traineeship will be based, this could be a different location to the organisation address. Use the place the trainee will spend most of their time.
        /// </summary>
        [JsonProperty("address")]
        public CreateTraineeshipVacancyAddress Address { get; set; }
        [JsonProperty("durationAndWorkingHours", Required = Required.Always)]
        public CreateTraineeshipDurationAndWorkingHours DurationAndWorkingHours { get; set; }
        /// <summary>
        /// Add the desired skills and personal qualities you’d like the applicant to have in order for you to consider them. There is a selection available from `GET referencedata/skills` or you can add your own. You must include at least one desired skill
        /// </summary>
        [JsonProperty("skills", Required = Required.Always)]
        public List<string> Skills { get; set; }
        /// <summary>
        /// Select if you do not wish your company name to be listed on the advert. This could mean fewer people view your advert.
        /// </summary>
        [JsonProperty("employerNameOption")]
        public TraineeshipEmployerNameOption EmployerNameOption { get; set; }
        /// <summary>
        /// Provide the reason why the organisation would like to remain anonymous if chosen for <see cref="EmployerNameOption"/>. The reason must not be more than 4000 characters
        /// </summary>
        [JsonProperty("anonymousReason")]
        public string AnonymousReason { get; set; }
        /// <summary>
        /// Are you registered as a Disability Confident employer?
        /// </summary>
        [JsonProperty("disabilityConfident", Required = Required.Always)]
        public CreateTraineeshipVacancyDisabilityConfident DisabilityConfident { get; set; }

        /// <summary>
        /// What work experience will the employer give the trainee?
        /// </summary>
        [JsonProperty("workexperience")]
        public string WorkExperience { get; set; }
        public static PostTraineeshipVacancyUserData Map(TraineeshipSubmitterContactDetails submitterContactDetails, TraineeshipContractingParties contractingParties)
        {
            return new PostTraineeshipVacancyUserData
            {
                Email = submitterContactDetails.Email,
                Name = submitterContactDetails.Name,
                Ukprn = contractingParties.Ukprn,
            };
        }
    }


    /// <summary>
    /// Address for the traineeship advert. Must contain address line 1 and a valid postcode.
    /// </summary>
    public class CreateTraineeshipVacancyAddress
    {
        public static implicit operator PostTraineeshipVacancyAddressData(CreateTraineeshipVacancyAddress source)
        {
            return new PostTraineeshipVacancyAddressData
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

    public class CreateTraineeshipDurationAndWorkingHours
    {
        public static implicit operator PostCreateTraineeshipVacancyWageData(CreateTraineeshipDurationAndWorkingHours source)
        {
            Enum.TryParse(typeof(InnerApi.Requests.TraineeshipDurationUnit), source.DurationUnit.ToString(), true,
                out var durationUnit);
            return new PostCreateTraineeshipVacancyWageData
            {
                WeeklyHours = source.WeeklyHours,
                Duration = source.Duration,
                WorkingWeekDescription = source.WorkingWeekDescription,
                DurationUnit = (InnerApi.Requests.TraineeshipDurationUnit)durationUnit,
            };
        }

        /// <summary>
        /// The total number of hours per week.
        /// </summary>
        [JsonProperty("weeklyHours", Required = Required.Always)]
        public decimal WeeklyHours { get; set; }
        /// <summary>
        /// Expected duration must be at least 6 weeks and up to 12 months.
        /// </summary>
        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }
        /// <summary>
        /// A short description of the pattern of working hours over the week. Start time, end time and working days. You have up to 250 characters
        /// </summary>
        [JsonProperty("workingWeekDescription", Required = Required.Always)]
        public string WorkingWeekDescription { get; set; }

        /// <summary>
        /// Used with <see cref="Duration"/> for duration in weeks or months
        /// </summary>
        [JsonProperty("durationUnit", Required = Required.Always)]
        public TraineeshipDurationUnit DurationUnit { get; set; }
    }

    public class TraineeshipSubmitterContactDetails
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

    public class TraineeshipContractingParties
    {
        /// <summary>
        /// The UKPRN of the training provider you will be working with for this traineeship.
        /// </summary>
        [JsonProperty("ukprn")]
        public int Ukprn { get; set; }
        /// <summary>
        /// The Account Legal Entity public hashed Id of the organisation that you wish to create the vacancy for. This can be obtained from `GET accountlegalentites`
        /// </summary>
        [Required]
        public string AccountLegalEntityPublicHashedId { get; set; }
    }

    public enum TraineeshipDurationUnit
    {
        Week,
        Month
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