using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.Vacancies.Application.Vacancies.Queries.GetVacancy;
using SFA.DAS.Vacancies.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.Models
{
    public record GetVacancyResponseV2 : GetVacanciesListResponseItemV2
    {
        /// <summary>
        /// A description of the company the apprentice will work at. Will be less than or equal to 4000 characters.
        /// </summary>
        [MaxLength(4000)]
        public string EmployerDescription { get; set; }
        /// <summary>
        /// The apprenticeship�s training plan, including where and when training will take place.
        /// </summary>
        public string TrainingDescription { get ; set ; }
        /// <summary>
        /// Additional information about the training, such as details about the provider.
        /// </summary>
        public string AdditionalTrainingDescription { get; set; }
        /// <summary>
        /// What progress or outcome the apprentice can expect at the end of the apprenticeship.
        /// </summary>
        public string OutcomeDescription { get ; set ; }
        public string FullDescription { get ; set ; }
        public List<string> Skills { get ; set ; }
        public List<GetVacancyQualification> Qualifications { get ; set ; }
        public string ThingsToConsider { get; set; }
        public string CompanyBenefitsInformation { get; set; }

        public static implicit operator GetVacancyResponseV2(GetVacancyApiResponse source)
        {
            if (source == null)
            {
                return null;
            }

            var isRecruitNationally = string.Equals(
                source.VacancyLocationType,
                "National",
                StringComparison.CurrentCultureIgnoreCase);

            var addresses = new List<GetVacancyAddressItem>();
            if (source.Address != null)
            {
                addresses.Add(GetVacancyAddressItem.From(source.Address));
            }
            if (source.OtherAddresses != null && source.OtherAddresses.Any())
            {
                addresses.AddRange(source.OtherAddresses.Select(GetVacancyAddressItem.From));
            }

            return new GetVacancyResponseV2
            {
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                Addresses = addresses,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                ApplicationUrl = source.ApplicationUrl,
                ClosingDate = source.ClosingDate.AddDays(1).Subtract(TimeSpan.FromSeconds(1)),
                CompanyBenefitsInformation = source.CompanyBenefitsInformation,
                Course = source,
                Description = source.Description,
                Distance = source.Distance,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactName = source.EmployerContactName,
                EmployerContactPhone = source.EmployerContactPhone,
                EmployerDescription = source.EmployerDescription,
                EmployerName = source.IsEmployerAnonymous ? source.AnonymousEmployerName : source.EmployerName,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                ExpectedDuration = source.ExpectedDuration,
                FullDescription = source.LongDescription,
                HoursPerWeek = source.HoursPerWeek,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsNationalVacancy = isRecruitNationally,
                IsNationalVacancyDetails = isRecruitNationally ? source.EmploymentLocationInformation : string.Empty,
                NumberOfPositions = source.NumberOfPositions,
                OutcomeDescription = source.OutcomeDescription,
                PostedDate = source.PostedDate,
                ProviderName = source.ProviderName,
                Qualifications = source.Qualifications.Select(c=>(GetVacancyQualification)c).ToList(),
                Skills = source.Skills,
                StartDate = source.StartDate,
                ThingsToConsider = source.ThingsToConsider,
                Title = source.Title,
                TrainingDescription = source.TrainingDescription,
                Ukprn = int.Parse(source.Ukprn),
                VacancyReference = source.VacancySource.Equals(DataSource.Raa) ? source.VacancyReference.TrimVacancyReference() : source.VacancyReference,
                VacancyUrl = source.VacancyUrl,
                Wage = source,
            };
        }
    }
}