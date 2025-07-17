using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class GetApprenticeshipVacancyApiResponse
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
        public bool IsSavedVacancy { get; set; } = false;

        [JsonProperty("VacancyQualification")]
        public IEnumerable<VacancyQualificationApiResponse> Qualifications { get; init; }

        public AddressApiResponse Address { get; init; }
        public List<AddressApiResponse>? OtherAddresses { get; init; } = [];
        public string? EmploymentLocationInformation { get; set; }
        
        [JsonProperty("availableWhere"), System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter<AvailableWhere>))]
        public AvailableWhere? EmployerLocationOption { get; set; }
        public List<string> CourseSkills { get; set; }
        public List<string> CourseCoreDuties { get; set; }
        public string CourseOverviewOfRole { get; set; }
        public string StandardPageUrl { get; set; }
        public string? CompanyBenefitsInformation { get; set; }
        public string? AdditionalTrainingDescription { get; set; }

        [JsonProperty("levels")] 
        public List<GetCourseLevelsListItem>? Levels { get; set; } = [];

        public CandidateApplication? Application { get; set; } = null;
        public string CandidatePostcode { get; set; }

        public string ApplicationUrl { get; set; }
        public string ApplicationInstructions { get; set; }
        public VacancyDataSource VacancySource { get; set; }
        public decimal? ApprenticeMinimumWage { get; set; }
        public decimal? Over25NationalMinimumWage { get; set; }
        public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
        public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
        public decimal? Under18NationalMinimumWage { get; set; }
        public DateTime? CandidateDateOfBirth { get; set; }
        public ApprenticeshipTypes ApprenticeshipType { get; init; }

        public static implicit operator GetApprenticeshipVacancyApiResponse(GetApprenticeshipVacancyQueryResult source)
        {
            return new GetApprenticeshipVacancyApiResponse
            {
                AdditionalTrainingDescription = source.ApprenticeshipVacancy.AdditionalTrainingDescription,
                Address = source.ApprenticeshipVacancy.Address,
                AnonymousEmployerName = source.ApprenticeshipVacancy.AnonymousEmployerName,
                Application = (CandidateApplication)source.Application,
                ApplicationInstructions = source.ApprenticeshipVacancy.ApplicationInstructions,
                ApplicationUrl = source.ApprenticeshipVacancy.ApplicationUrl,
                ApprenticeMinimumWage = source.ApprenticeshipVacancy.ApprenticeMinimumWage,
                ApprenticeshipLevel = source.ApprenticeshipVacancy.ApprenticeshipLevel,
                ApprenticeshipType = source.ApprenticeshipVacancy.ApprenticeshipType,
                Between18AndUnder21NationalMinimumWage = source.ApprenticeshipVacancy.Between18AndUnder21NationalMinimumWage,
                Between21AndUnder25NationalMinimumWage = source.ApprenticeshipVacancy.Between21AndUnder25NationalMinimumWage,
                CandidateDateOfBirth = source.CandidateDateOfBirth,
                CandidatePostcode = source.CandidatePostcode,
                Category = source.ApprenticeshipVacancy.Category,
                CategoryCode = source.ApprenticeshipVacancy.CategoryCode,
                ClosingDate = source.ApprenticeshipVacancy.ClosingDate,
                CompanyBenefitsInformation = source.ApprenticeshipVacancy.CompanyBenefitsInformation,
                CourseCoreDuties = source.CourseDetail?.CoreDuties ?? [],
                CourseId = source.ApprenticeshipVacancy.CourseId,
                CourseLevel = source.ApprenticeshipVacancy.CourseLevel,
                CourseOverviewOfRole = source.CourseDetail?.OverviewOfRole ?? string.Empty,
                CourseRoute = source.ApprenticeshipVacancy.CourseRoute,
                CourseSkills = source.CourseDetail?.Skills ?? [],
                CourseTitle = source.ApprenticeshipVacancy.CourseTitle,
                Description = source.ApprenticeshipVacancy.Description,
                Distance = source.ApprenticeshipVacancy.Distance,
                EmployerContactEmail = source.ApprenticeshipVacancy.EmployerContactEmail,
                EmployerContactName = source.ApprenticeshipVacancy.EmployerContactName,
                EmployerContactPhone = source.ApprenticeshipVacancy.EmployerContactPhone,
                EmployerDescription = source.ApprenticeshipVacancy.EmployerDescription,
                EmployerLocationOption = source.ApprenticeshipVacancy.EmployerLocationOption,
                EmployerName = source.ApprenticeshipVacancy.EmployerName,
                EmployerWebsiteUrl = source.ApprenticeshipVacancy.EmployerWebsiteUrl,
                EmploymentLocationInformation = source.ApprenticeshipVacancy.EmploymentLocationInformation,
                ExpectedDuration = source.ApprenticeshipVacancy.ExpectedDuration,
                FrameworkLarsCode = source.ApprenticeshipVacancy.FrameworkLarsCode,
                HoursPerWeek = source.ApprenticeshipVacancy.HoursPerWeek,
                Id = source.ApprenticeshipVacancy.Id,
                IsClosed = source.ApprenticeshipVacancy.IsClosed,
                IsDisabilityConfident = source.ApprenticeshipVacancy.IsDisabilityConfident,
                IsEmployerAnonymous = source.ApprenticeshipVacancy.IsEmployerAnonymous,
                IsPositiveAboutDisability = source.ApprenticeshipVacancy.IsPositiveAboutDisability,
                IsRecruitVacancy = source.ApprenticeshipVacancy.IsRecruitVacancy,
                IsSavedVacancy = source.IsSavedVacancy,
                Levels = source.Levels,
                Location = source.ApprenticeshipVacancy.Location,
                LongDescription = source.ApprenticeshipVacancy.LongDescription,
                NumberOfPositions = source.ApprenticeshipVacancy.NumberOfPositions,
                OtherAddresses = source.ApprenticeshipVacancy.OtherAddresses?.Select(a => (AddressApiResponse) a).ToList(),
                OutcomeDescription = source.ApprenticeshipVacancy.OutcomeDescription,
                Over25NationalMinimumWage = source.ApprenticeshipVacancy.Over25NationalMinimumWage,
                PostedDate = source.ApprenticeshipVacancy.PostedDate,
                ProviderContactEmail = source.ApprenticeshipVacancy.ProviderContactEmail,
                ProviderContactName = source.ApprenticeshipVacancy.ProviderContactName,
                ProviderContactPhone = source.ApprenticeshipVacancy.ProviderContactPhone,
                ProviderName = source.ApprenticeshipVacancy.ProviderName,
                Qualifications = source.ApprenticeshipVacancy.Qualifications?.Select(l => (VacancyQualificationApiResponse) l),
                Score = source.ApprenticeshipVacancy.Score,
                Skills = source.ApprenticeshipVacancy.Skills,
                StandardLarsCode = source.ApprenticeshipVacancy.CourseId,
                StandardPageUrl = source.CourseDetail?.StandardPageUrl ?? string.Empty,
                StartDate = source.ApprenticeshipVacancy.StartDate,
                SubCategory = source.ApprenticeshipVacancy.SubCategory,
                SubCategoryCode = source.ApprenticeshipVacancy.SubCategoryCode,
                ThingsToConsider = source.ApprenticeshipVacancy.ThingsToConsider,
                Title = source.ApprenticeshipVacancy.Title,
                TrainingDescription = source.ApprenticeshipVacancy.TrainingDescription,
                Ukprn = source.ApprenticeshipVacancy.Ukprn,
                Under18NationalMinimumWage = source.ApprenticeshipVacancy.Under18NationalMinimumWage,
                VacancyLocationType = source.ApprenticeshipVacancy.VacancyLocationType,
                VacancyReference = source.ApprenticeshipVacancy.VacancyReference,
                VacancySource = source.ApprenticeshipVacancy.VacancySource,
                WageAdditionalInformation = source.ApprenticeshipVacancy.WageAdditionalInformation,
                WageAmount = source.ApprenticeshipVacancy.WageAmount,
                WageAmountLowerBound = source.ApprenticeshipVacancy.WageAmountLowerBound,
                WageAmountUpperBound = source.ApprenticeshipVacancy.WageAmountUpperBound,
                WageText = source.ApprenticeshipVacancy.WageText,
                WageType = source.ApprenticeshipVacancy.WageType,
                WageUnit = source.ApprenticeshipVacancy.WageUnit,
                WorkingWeek = source.ApprenticeshipVacancy.WorkingWeek,
            };
        }
    }

    public class VacancyQualificationApiResponse
    {
        public string QualificationType { get; init; }
        public string Subject { get; init; }
        public string Grade { get; init; }
        public Weighting Weighting { get; init; }

        public static implicit operator VacancyQualificationApiResponse(GetApprenticeshipVacancyQueryResult.VacancyQualification source)
        {
            return new VacancyQualificationApiResponse
            {
                Grade = source.Grade,
                Subject = source.Subject,
                QualificationType = source.QualificationType,
                Weighting = source.Weighting,
            };
        }
    }

    public class AddressApiResponse
    {
        public string AddressLine1 { get; init; }
        public string AddressLine2 { get; init; }
        public string AddressLine3 { get; init; }
        public string AddressLine4 { get; init; }
        public string Postcode { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }

        public static implicit operator AddressApiResponse(Address source)
        {
            if (source is null)
            {
                return null;
            }

            return new AddressApiResponse
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
            };
        }
    }

    public class CandidateApplication
    {
        public string Status { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? WithdrawnDate { get; set; }
        public Guid ApplicationId { get; set; }

        public static implicit operator CandidateApplication(GetApprenticeshipVacancyQueryResult.CandidateApplication source)
        {
            if (source is null) return null;

            return new CandidateApplication
            {
                SubmittedDate = source.SubmittedDate,
                WithdrawnDate = source.WithdrawnDate,
                Status = source.Status,
                ApplicationId = source.ApplicationId
            };
        }
    }
}