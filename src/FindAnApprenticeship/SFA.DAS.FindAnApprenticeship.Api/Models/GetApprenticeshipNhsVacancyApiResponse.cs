using SFA.DAS.FindAnApprenticeship.Application.Queries.GetVacancyDetails;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetApprenticeshipNhsVacancyApiResponse
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string VacancyReference { get; init; }
        public string LongDescription { get; init; }
        public string OutcomeDescription { get; init; }
        public string TrainingDescription { get; init; }
        public string ThingsToConsider { get; init; }
        public IEnumerable<string> Skills { get; init; }

        public DateTime StartDate { get; init; }
        public DateTime ClosingDate { get; init; }
        public DateTime PostedDate { get; init; }


        public string WageAmount { get; init; }
        public int WageType { get; init; }
        public decimal? WageAmountLowerBound { get; init; }
        public decimal? WageAmountUpperBound { get; init; }
        public string WageText { get; init; }
        public int WageUnit { get; init; }
        public string WageAdditionalInformation { get; init; }

        public decimal? Distance { get; init; }

        public string CourseTitle { get; init; }
        public string CourseLevel { get; init; }
        public int CourseId { get; init; }
        public string CourseRoute { get; init; }
        public string ApprenticeshipLevel { get; init; }

        public string Category { get; init; }
        public string CategoryCode { get; init; }
        public string SubCategory { get; init; }
        public string SubCategoryCode { get; init; }

        public string Description { get; init; }
        public string FrameworkLarsCode { get; init; }
        public decimal? HoursPerWeek { get; init; }
        public bool IsDisabilityConfident { get; init; }
        public bool IsPositiveAboutDisability { get; init; }
        public bool IsRecruitVacancy { get; init; }
        public Location Location { get; init; }
        public int NumberOfPositions { get; init; }
        public string ProviderName { get; init; }
        public int? StandardLarsCode { get; init; }


        public VacancyLocationType? VacancyLocationType { get; init; }

        public VacancyDataSource VacancyDataSource { get;init; }

        public string WorkingWeek { get; init; }
        public string ExpectedDuration { get; init; }
        public double Score { get; init; }

        public string Ukprn { get; init; }
        public string EmployerName { get; init; }
        public string EmployerDescription { get; init; }
        public string EmployerWebsiteUrl { get; init; }
        public string EmployerContactPhone { get; init; }
        public string EmployerContactEmail { get; init; }
        public string EmployerContactName { get; init; }
        public string ProviderContactPhone { get; init; }
        public string ProviderContactEmail { get; init; }
        public string ProviderContactName { get; init; }

        public string AnonymousEmployerName { get; init; }
        public bool IsEmployerAnonymous { get; init; }
        public bool IsClosed { get; set; }

        public AddressApiResponse Address { get; init; }
        
        public string? CompanyBenefitsInformation { get; set; }
        public string? AdditionalTrainingDescription { get; set; }

        public string ApplicationUrl { get; set; }
        public string ApplicationInstructions { get; set; }
        
        public static implicit operator GetApprenticeshipNhsVacancyApiResponse(GetVacancyDetailsQueryResult source)
        {
            return new GetApprenticeshipNhsVacancyApiResponse
            {
                LongDescription = source.ApprenticeshipVacancy.LongDescription,
                OutcomeDescription = source.ApprenticeshipVacancy.OutcomeDescription,
                TrainingDescription = source.ApprenticeshipVacancy.TrainingDescription,
                ThingsToConsider = source.ApprenticeshipVacancy.ThingsToConsider,
                Skills = source.ApprenticeshipVacancy.Skills,
                Id = source.ApprenticeshipVacancy.Id,
                ClosingDate = source.ApprenticeshipVacancy.ClosingDate,
                PostedDate = source.ApprenticeshipVacancy.PostedDate,
                EmployerName = source.ApprenticeshipVacancy.EmployerName,
                Title = source.ApprenticeshipVacancy.Title,
                VacancyReference = source.ApprenticeshipVacancy.VacancyReference,
                CourseTitle = source.ApprenticeshipVacancy.CourseTitle,
                WageAmount = source.ApprenticeshipVacancy.WageAmount,
                WageType = source.ApprenticeshipVacancy.WageType,
                Distance = source.ApprenticeshipVacancy.Distance,
                CourseLevel = source.ApprenticeshipVacancy.CourseLevel,
                CourseId = source.ApprenticeshipVacancy.CourseId,
                CourseRoute = source.ApprenticeshipVacancy.CourseRoute,
                ApprenticeshipLevel = source.ApprenticeshipVacancy.ApprenticeshipLevel,

                Category = source.ApprenticeshipVacancy.Category,
                CategoryCode = source.ApprenticeshipVacancy.CategoryCode,
                Description = source.ApprenticeshipVacancy.Description,
                FrameworkLarsCode = source.ApprenticeshipVacancy.FrameworkLarsCode,
                HoursPerWeek = source.ApprenticeshipVacancy.HoursPerWeek,
                IsDisabilityConfident = source.ApprenticeshipVacancy.IsDisabilityConfident,
                IsPositiveAboutDisability = source.ApprenticeshipVacancy.IsPositiveAboutDisability,
                IsRecruitVacancy = source.ApprenticeshipVacancy.IsRecruitVacancy,
                Location = source.ApprenticeshipVacancy.Location,
                NumberOfPositions = source.ApprenticeshipVacancy.NumberOfPositions,
                ProviderName = source.ApprenticeshipVacancy.ProviderName,
                StandardLarsCode = source.ApprenticeshipVacancy.CourseId,
                StartDate = source.ApprenticeshipVacancy.StartDate,
                SubCategory = source.ApprenticeshipVacancy.SubCategory,
                SubCategoryCode = source.ApprenticeshipVacancy.SubCategoryCode,
                Ukprn = source.ApprenticeshipVacancy.Ukprn,
                VacancyLocationType = source.ApprenticeshipVacancy.VacancyLocationType,
                WageAmountLowerBound = source.ApprenticeshipVacancy.WageAmountLowerBound,
                WageAmountUpperBound = source.ApprenticeshipVacancy.WageAmountUpperBound,
                WageText = source.ApprenticeshipVacancy.WageText,
                WageUnit = source.ApprenticeshipVacancy.WageUnit,
                WageAdditionalInformation = source.ApprenticeshipVacancy.WageAdditionalInformation,
                WorkingWeek = source.ApprenticeshipVacancy.WorkingWeek,
                ExpectedDuration = source.ApprenticeshipVacancy.ExpectedDuration,
                Score = source.ApprenticeshipVacancy.Score,
                EmployerDescription = source.ApprenticeshipVacancy.EmployerDescription,
                EmployerContactEmail = source.ApprenticeshipVacancy.EmployerContactEmail,
                EmployerContactName = source.ApprenticeshipVacancy.EmployerContactName,
                EmployerContactPhone = source.ApprenticeshipVacancy.EmployerContactPhone,
                ProviderContactEmail = source.ApprenticeshipVacancy.ProviderContactEmail,
                ProviderContactName = source.ApprenticeshipVacancy.ProviderContactName,
                ProviderContactPhone = source.ApprenticeshipVacancy.ProviderContactPhone,
                EmployerWebsiteUrl = source.ApprenticeshipVacancy.EmployerWebsiteUrl,
                AnonymousEmployerName = source.ApprenticeshipVacancy.AnonymousEmployerName,
                IsEmployerAnonymous = source.ApprenticeshipVacancy.IsEmployerAnonymous,
                Address = source.ApprenticeshipVacancy.Address,
				IsClosed = source.ApprenticeshipVacancy.IsClosed,
                ApplicationUrl = source.ApprenticeshipVacancy.ApplicationUrl,
                ApplicationInstructions = source.ApprenticeshipVacancy.ApplicationInstructions,
                CompanyBenefitsInformation = source.ApprenticeshipVacancy.CompanyBenefitsInformation,
                AdditionalTrainingDescription = source.ApprenticeshipVacancy.AdditionalTrainingDescription,
                VacancyDataSource = source.ApprenticeshipVacancy.VacancySource
            };
        }
    }
}