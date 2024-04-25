using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryResult
    {
        public Vacancy ApprenticeshipVacancy { get; init; }
        public GetStandardsListItemResponse CourseDetail { get; init; }
        public List<GetCourseLevelsListItem> Levels { get; init; }

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
            public GeoPoint Location { get; init; }
            public int NumberOfPositions { get; init; }
            public string ProviderName { get; init; }
            public DateTime StartDate { get; init; }
            public string SubCategory { get; init; }
            public string SubCategoryCode { get; init; }
            public string Ukprn { get; init; }
            
            public decimal? WageAmountLowerBound { get; init; }
            
            public decimal? WageAmountUpperBound { get; init; }
            
            public string WageText { get; init; }
            
            public int WageUnit { get; init; }
            
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

            public VacancyLocationType VacancyLocationType { get; init; }

            public IEnumerable<string> Skills { get; init; }

            public IEnumerable<VacancyQualification> Qualifications { get; init; }

            public string AdditionalQuestion1 { get; init; }

            public string AdditionalQuestion2 { get; init; }

            public bool IsClosed { get; set; }

            public static implicit operator Vacancy(GetClosedVacancyResponse source)
            {
                return new Vacancy
                {
                    EmployerName = source.EmployerName,
                    Title = source.Title,
                    ClosingDate = source.ClosingDate,
                    IsClosed = true,
                    Address = new Address
                    {
                        AddressLine1 = source.EmployerLocation?.AddressLine1,
                        AddressLine2 = source.EmployerLocation?.AddressLine2,
                        AddressLine3 = source.EmployerLocation?.AddressLine3,
                        AddressLine4 = source.EmployerLocation?.AddressLine4,
                        Postcode = source.EmployerLocation?.Postcode
                    }
                };
            }

            public static implicit operator Vacancy(GetApprenticeshipVacancyItemResponse source)
            {
                return new Vacancy
                {
                    Id = source.Id,
                    AnonymousEmployerName = source.AnonymousEmployerName,
                    ApprenticeshipLevel = source.ApprenticeshipLevel,
                    ClosingDate = source.ClosingDate,
                    EmployerName = source.EmployerName,
                    IsEmployerAnonymous = source.IsEmployerAnonymous,
                    PostedDate = source.PostedDate,
                    Title = source.Title,
                    VacancyReference = source.VacancyReference,
                    CourseTitle = source.CourseTitle,
                    CourseId = source.CourseId,
                    WageAmount = source.WageAmount,
                    WageType = source.WageType,
                    Address = source.Address,
                    Distance = source.Distance,
                    CourseRoute = source.CourseRoute,
                    CourseLevel = source.CourseLevel,
                    LongDescription = source.LongDescription,
                    OutcomeDescription = source.OutcomeDescription,
                    TrainingDescription = source.TrainingDescription,
                    ThingsToConsider = source.ThingsToConsider,
                    Category = source.Category,
                    CategoryCode = source.CategoryCode,
                    Description = source.Description,
                    FrameworkLarsCode = source.FrameworkLarsCode,
                    HoursPerWeek = source.HoursPerWeek,
                    IsDisabilityConfident = source.IsDisabilityConfident,
                    IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                    IsRecruitVacancy = source.IsRecruitVacancy,
                    Location = source.Location,
                    NumberOfPositions = source.NumberOfPositions,
                    ProviderName = source.ProviderName,
                    StartDate = source.StartDate,
                    SubCategory = source.SubCategory,
                    SubCategoryCode = source.SubCategoryCode,
                    Ukprn = source.Ukprn,
                    WageAmountLowerBound = source.WageAmountLowerBound,
                    WageAmountUpperBound = source.WageAmountUpperBound,
                    WageText = source.WageText,
                    WageUnit = source.WageUnit,
                    WorkingWeek = source.WorkingWeek,
                    ExpectedDuration = source.ExpectedDuration,
                    Score = source.Score,
                    EmployerDescription = source.EmployerDescription,
                    EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                    EmployerContactPhone = source.EmployerContactPhone,
                    EmployerContactEmail = source.EmployerContactEmail,
                    EmployerContactName = source.EmployerContactName,
                    ProviderContactPhone = source.ProviderContactPhone,
                    ProviderContactEmail = source.ProviderContactEmail,
                    ProviderContactName = source.ProviderContactName,
                    VacancyLocationType = source.VacancyLocationType,
                    Skills = source.Skills,
                    Qualifications = source.Qualifications?.Select(x => new VacancyQualification
                    {
                        QualificationType = x.QualificationType,
                        Subject = x.Subject,
                        Grade = x.Grade,
                        Weighting = x.Weighting
                    }),
                    AdditionalQuestion1 = source.AdditionalQuestion1,
                    AdditionalQuestion2 = source.AdditionalQuestion2,
                    IsClosed = false
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
    }
}