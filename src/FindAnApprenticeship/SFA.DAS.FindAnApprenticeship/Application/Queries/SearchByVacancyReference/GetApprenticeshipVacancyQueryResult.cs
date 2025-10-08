using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Microsoft.OpenApi.Extensions;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryResult
    {
        public Vacancy ApprenticeshipVacancy { get; init; }
        public GetStandardsListItemResponse CourseDetail { get; set; }
        public List<GetCourseLevelsListItem> Levels { get; init; }
        public CandidateApplication Application { get; init; }
        public string CandidatePostcode { get; set; }
        public bool IsSavedVacancy { get; set; }
        public DateTime? CandidateDateOfBirth { get; set; }

        public class Vacancy
        {
            public string Id { get; set; }
            public string AnonymousEmployerName { get; set; }
            public string ApprenticeshipLevel { get; set; }
            public DateTime ClosingDate { get; set; }
            public string EmployerName { get; set; }
            public bool IsEmployerAnonymous { get; set; }
            public DateTime PostedDate { get; set; }
            public string Title { get; set; }
            public string VacancyReference { get; set; }
            public string CourseTitle { get; set; }
            public int CourseId { get; set; }
            public string WageAmount { get; set; }
            public int WageType { get; set; }
            public Address Address { get; set; }
            public List<Address>? OtherAddresses { get; set; }
            public string? EmploymentLocationInformation { get; set; }
            public AvailableWhere? EmployerLocationOption { get; set; }
            public decimal? Distance { get; set; }
            public string CourseRoute { get; set; }
            public string CourseLevel { get; set; }
            public string LongDescription { get; init; }
            public string OutcomeDescription { get; init; }
            public string TrainingDescription { get; init; }
            public string ThingsToConsider { get; init; }
            public string Category { get; init; }
            public string CategoryCode { get; init; }
            public string Description { get; init; }
            public string FrameworkLarsCode { get; init; }
            public decimal? HoursPerWeek { get; init; }
            public bool IsDisabilityConfident { get; init; }
            public bool IsPositiveAboutDisability { get; init; }
            public bool IsRecruitVacancy { get; init; }
            public Location Location { get; init; }
            public int NumberOfPositions { get; init; }
            public string ProviderName { get; init; }
            public DateTime StartDate { get; init; }
            public string SubCategory { get; init; }
            public string SubCategoryCode { get; init; }
            public string Ukprn { get; init; }
            
            public decimal? WageAmountLowerBound { get; init; }
            
            public decimal? WageAmountUpperBound { get; init; }
            public string WageText { get; init; }
            public decimal? ApprenticeMinimumWage { get; set; }
            public decimal? Over25NationalMinimumWage { get; set; }
            public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
            public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
            public decimal? Under18NationalMinimumWage { get; set; }
            public int WageUnit { get; init; }
            public string WageAdditionalInformation { get; set; }

            public string WorkingWeek { get; init; }
            
            public string ExpectedDuration { get; init; }
            public double Score { get; init; }
            
            public string EmployerDescription { get; init; }
            
            public string EmployerWebsiteUrl { get; init; }
            
            public string EmployerContactPhone { get; init; }
            
            public string EmployerContactEmail { get; init; }
            
            public string EmployerContactName { get; init; }
            
            public string ProviderContactPhone { get; init; }

            public string ProviderContactEmail { get; init; }

            public string ProviderContactName { get; init; }

            public VacancyLocationType? VacancyLocationType { get; init; }

            public IEnumerable<string> Skills { get; init; }

            public IEnumerable<VacancyQualification> Qualifications { get; init; }

            public string AdditionalQuestion1 { get; init; }

            public string AdditionalQuestion2 { get; init; }

            public bool IsClosed { get; set; }
            public string? CompanyBenefitsInformation { get; set; }
            public string? AdditionalTrainingDescription { get; set; }
            public string ApplicationUrl { get; set; }
            public string ApplicationInstructions { get; set; }
            public VacancyDataSource VacancySource { get; set; }
            public ApprenticeshipTypes ApprenticeshipType { get; init; }

            public static Vacancy FromIVacancy(IVacancy source, GetStandardsListItemResponse courseResult = null)
            {
                return source switch
                {
                    GetClosedVacancyResponse response => From(response, courseResult),
                    GetApprenticeshipVacancyItemResponse response => From(response),
                    _ => throw new InvalidCastException()
                };
            }

            private static Vacancy From(GetClosedVacancyResponse source, GetStandardsListItemResponse courseResult)
            {
                var durationUnit = source.Wage.DurationUnit ?? default;

                return new Vacancy
                {
                    AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                    AdditionalQuestion1 = source.AdditionalQuestion1,
                    AdditionalQuestion2 = source.AdditionalQuestion2,
                    Address = source.Address,
                    OtherAddresses = source.OtherAddresses,
                    EmploymentLocationInformation = source.EmploymentLocationInformation,
                    EmployerLocationOption = source.EmployerLocationOption,
                    AnonymousEmployerName = source.IsAnonymous ? source.EmployerName : null,
                    ApplicationInstructions = source.ApplicationInstructions,
                    ApplicationUrl = source.ApplicationUrl,
                    ApprenticeshipLevel = GetApprenticeshipLevel(courseResult?.Level),
                    ApprenticeshipType = source.ApprenticeshipType,
                    ClosingDate = source.ClosedDate ?? source.ClosingDate,
                    CourseId = source.CourseId,
                    CourseLevel = courseResult?.Level.ToString(),
                    CourseTitle = courseResult?.Title,
                    Description = source.ShortDescription,
                    EmployerContactEmail = source.EmployerContact?.Email,
                    EmployerContactName = source.EmployerContact?.Name,
                    EmployerContactPhone = source.EmployerContact?.Phone,
                    EmployerDescription = source.EmployerDescription,
                    EmployerName = source.EmployerName,
                    EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                    ExpectedDuration = durationUnit.GetDisplayName().ToLower().ToQuantity(source.Wage.Duration),
                    HoursPerWeek = source.Wage.WeeklyHours,
                    Id = source.VacancyReference.TrimVacancyReference(),
                    IsClosed = source.ClosedDate.HasValue,
                    IsDisabilityConfident = source.IsDisabilityConfident,
                    IsEmployerAnonymous = source.IsAnonymous,
                    IsPositiveAboutDisability = false,
                    IsRecruitVacancy = true,
                    Location = new Location
                    {
                        Lat = source.Address?.Latitude ?? 0,
                        Lon = source.Address?.Longitude ?? 0,
                    },
                    LongDescription = source.Description,
                    NumberOfPositions = source.NumberOfPositions,
                    OutcomeDescription = source.OutcomeDescription,
                    PostedDate = source.LiveDate,
                    ProviderContactEmail = source.ProviderContact?.Email,
                    ProviderContactName = source.ProviderContact?.Name,
                    ProviderContactPhone = source.ProviderContact?.Phone,
                    ProviderName = source.TrainingProvider.Name,
                    Qualifications = source.Qualifications.Select(q => new VacancyQualification
                    {
                        QualificationType = q.QualificationType,
                        Subject = q.Subject,
                        Grade = q.Grade,
                        Weighting = q.Weighting != null ? (Weighting)q.Weighting : default
                    }),
                    Skills = source.Skills,
                    StartDate = source.StartDate,
                    ThingsToConsider = source.ThingsToConsider,
                    Title = source.Title,
                    TrainingDescription = source.TrainingDescription,
                    Ukprn = source.TrainingProvider.Ukprn.ToString(),
                    VacancyLocationType = source.VacancyLocationType,
                    VacancyReference = source.VacancyReference.TrimVacancyReference(),
                    WageAdditionalInformation = source.Wage.WageAdditionalInformation,
                    WageType = (short)(source.Wage.WageType ?? 0),
                    WageUnit = (short)(source.Wage.DurationUnit ?? 0),
                    WageText = source.Wage.ToDisplayText(source.StartDate),
                    WorkingWeek = source.Wage.WorkingWeekDescription,
                    VacancySource = source.VacancySource,
                };
            }
            
            private static string GetApprenticeshipLevel(int? level)
            {
                return level switch
                {
                    0 => "Unknown",
                    2 => "Intermediate",
                    3 => "Advanced",
                    4 => "Higher",
                    5 => "Higher",
                    6 => "Degree",
                    7 => "Degree",
                    _ => string.Empty
                };
            }

            private static Vacancy From(GetApprenticeshipVacancyItemResponse source)
            {
                return new Vacancy
                {
                    AdditionalQuestion1 = source.AdditionalQuestion1,
                    AdditionalQuestion2 = source.AdditionalQuestion2,
                    AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                    Address = source.Address,
                    AnonymousEmployerName = source.AnonymousEmployerName,
                    ApplicationInstructions = source.ApplicationInstructions,
                    ApplicationUrl = source.ApplicationUrl,
                    ApprenticeshipLevel = source.ApprenticeshipLevel,
                    ApprenticeshipType = source.ApprenticeshipType,
                    Category = source.Category,
                    CategoryCode = source.CategoryCode,
                    ClosingDate = source.ClosingDate,
                    CompanyBenefitsInformation = source.CompanyBenefitsInformation,
                    CourseId = source.CourseId,
                    CourseLevel = source.CourseLevel,
                    CourseRoute = source.CourseRoute,
                    CourseTitle = source.CourseTitle,
                    Description = source.Description,
                    Distance = source.Distance,
                    EmployerContactEmail = source.EmployerContactEmail,
                    EmployerContactName = source.EmployerContactName,
                    EmployerContactPhone = source.EmployerContactPhone,
                    EmployerDescription = source.EmployerDescription,
                    EmployerName = source.EmployerName,
                    EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                    ExpectedDuration = source.ExpectedDuration,
                    FrameworkLarsCode = source.FrameworkLarsCode,
                    HoursPerWeek = source.HoursPerWeek,
                    Id = source.Id,
                    IsClosed = false,
                    IsDisabilityConfident = source.IsDisabilityConfident,
                    IsEmployerAnonymous = source.IsEmployerAnonymous,
                    IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                    IsRecruitVacancy = source.IsRecruitVacancy,
                    Location = source.Location,
                    LongDescription = source.LongDescription,
                    NumberOfPositions = source.NumberOfPositions,
                    OtherAddresses = source.OtherAddresses,
                    EmployerLocationOption = source.EmployerLocationOption,
                    EmploymentLocationInformation = source.EmploymentLocationInformation,
                    OutcomeDescription = source.OutcomeDescription,
                    PostedDate = source.PostedDate,
                    ProviderContactEmail = source.ProviderContactEmail,
                    ProviderContactName = source.ProviderContactName,
                    ProviderContactPhone = source.ProviderContactPhone,
                    ProviderName = source.ProviderName,
                    Qualifications = source.Qualifications?.Select(x => new VacancyQualification
                    {
                        QualificationType = x.QualificationType,
                        Subject = x.Subject,
                        Grade = x.Grade,
                        Weighting = x.Weighting
                    }),
                    Score = source.Score,
                    Skills = source.Skills,
                    StartDate = source.StartDate,
                    SubCategory = source.SubCategory,
                    SubCategoryCode = source.SubCategoryCode,
                    ThingsToConsider = source.ThingsToConsider,
                    Title = source.Title,
                    TrainingDescription = source.TrainingDescription,
                    Ukprn = source.Ukprn,
                    VacancyLocationType = source.VacancyLocationType,
                    VacancyReference = source.VacancyReference,
                    VacancySource = source.VacancySource,
                    WageAdditionalInformation = source.WageAdditionalInformation,
                    WageAmount = source.WageAmount,
                    WageAmountLowerBound = source.WageAmountLowerBound,
                    WageAmountUpperBound = source.WageAmountUpperBound,
                    WageText = source.WageText,
                    WageType = source.WageType,
                    WageUnit = (int)source.WageUnit,
                    WorkingWeek = source.WorkingWeek,
                    Under18NationalMinimumWage = source.Under18NationalMinimumWage,
                    Between18AndUnder21NationalMinimumWage = source.Between18AndUnder21NationalMinimumWage,
                    Between21AndUnder25NationalMinimumWage = source.Between21AndUnder25NationalMinimumWage,
                    Over25NationalMinimumWage = source.Over25NationalMinimumWage,
                    ApprenticeMinimumWage = source.ApprenticeMinimumWage
                };
            }
        }

        public class VacancyQualification
        {
            public string QualificationType { get; init; }
            public string Subject { get; init; }
            public string Grade { get; init; }
            public Weighting Weighting { get; init; }
        }
		
		public class CandidateApplication
        {
            public string Status { get; set; }
            public DateTime? SubmittedDate { get; set; }
            public DateTime? WithdrawnDate { get; set; }
            public Guid ApplicationId { get; set; }
        }
    }
}