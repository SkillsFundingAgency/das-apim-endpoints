using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.NhsJobs;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Models;

public class GetLiveVacanciesApiResponse
{
    public static implicit operator GetLiveVacanciesApiResponse(GetLiveVacanciesQueryResult source)
    {
        return new GetLiveVacanciesApiResponse
        {
            Vacancies = source.Vacancies.Select(x => (LiveVacancy)x),
            PageSize = source.PageSize,
            PageNo = source.PageNo,
            TotalLiveVacanciesReturned = source.TotalLiveVacanciesReturned,
            TotalLiveVacancies = source.TotalLiveVacancies,
            TotalPages = source.TotalPages
        };
    }

    public static implicit operator GetLiveVacanciesApiResponse(GetNhsJobsQueryResult source)
    {
        return new GetLiveVacanciesApiResponse
        {
            Vacancies = source.NhsVacancies.Select(x => (LiveVacancy)x),
            PageSize = source.NhsVacancies.Count,
            PageNo = 1,
            TotalLiveVacanciesReturned = source.NhsVacancies.Count,
            TotalLiveVacancies = source.NhsVacancies.Count,
            TotalPages = 1
        };
    }

    public static implicit operator GetLiveVacanciesApiResponse(GetCivilServiceJobsQueryResult source)
    {
        return new GetLiveVacanciesApiResponse
        {
            Vacancies = source.CivilServiceVacancies.Select(x => (LiveVacancy)x),
            PageSize = source.CivilServiceVacancies.Count,
            PageNo = 1,
            TotalLiveVacanciesReturned = source.CivilServiceVacancies.Count,
            TotalLiveVacancies = source.CivilServiceVacancies.Count,
            TotalPages = 1
        };
    }

    public IEnumerable<LiveVacancy> Vacancies { get; set; } = null!;
    public int PageSize { get; set; }
    public int PageNo { get; set; }
    public int TotalLiveVacanciesReturned { get; set; }
    public int TotalLiveVacancies { get; set; }
    public int TotalPages { get; set; }

    public class LiveVacancy
    {
        public static implicit operator LiveVacancy(Application.Shared.LiveVacancy source)
        {
            return new LiveVacancy
            {
                VacancyId = source.VacancyId,
                VacancyReference = source.VacancyReference,
                Title = source.Title,
                NumberOfPositions = source.NumberOfPositions,
                ApprenticeshipTitle = source.ApprenticeshipTitle,
                StandardLarsCode = source.StandardLarsCode,
                Route = source.Route,
                Description = source.Description,
                Address = source.Address,
                OtherAddresses = source.OtherAddresses,
                EmploymentLocations = source.EmploymentLocations,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                EmploymentLocationOption = source.EmploymentLocationOption,
                ClosingDate = source.ClosingDate,
                StartDate = source.StartDate,
                PostedDate = source.PostedDate,
                EmployerName = source.EmployerName,
                Ukprn = source.Ukprn,
                ProviderName = source.ProviderName,
                Level = source.Level,
                Wage = source.Wage == null ? null : (Wage)source.Wage,
                OutcomeDescription = source.OutcomeDescription,
                LongDescription = source.LongDescription,
                TrainingDescription = source.TrainingDescription,
                Skills = source.Skills,
                Qualifications = source.Qualifications.Select(q => new Qualification
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = q.Weighting
                }).ToList(),
                ThingsToConsider = source.ThingsToConsider,
                Id = source.Id,
                IsDisabilityConfident = source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                EmployerDescription = source.EmployerDescription,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                IsRecruitVacancy = source.IsRecruitVacancy,
                AnonymousEmployerName = source.AnonymousEmployerName,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                VacancyLocationType = source.VacancyLocationType,
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone,
                ProviderContactEmail = source.ProviderContactEmail,
                ProviderContactName = source.ProviderContactName,
                ProviderContactPhone = source.ProviderContactPhone,
                ApprenticeshipLevel = source.ApprenticeshipLevel,
                Duration = source.Duration,
                DurationUnit = source.DurationUnit,
                RouteCode = source.RouteCode,
                AccountId = source.AccountId,
                AccountLegalEntityId = source.AccountLegalEntityId,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationUrl = source.ApplicationUrl,
                ApplicationInstructions = source.ApplicationInstructions,
                TypicalJobTitles = source.TypicalJobTitles,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
                AdditionalTrainingDescription = source.AdditionalTrainingDescription,
                SearchTags = source.SearchTags,
                ApprenticeshipType = source.ApprenticeshipType,
            };
        }

        public string? ApplicationInstructions { get; set; }

        public int RouteCode { get; set; }
        public DurationUnit? DurationUnit { get; set; }
        public int Duration { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string ApprenticeshipLevel { get; set; }
        public Guid VacancyId { get; set; }
        public string VacancyReference { get; set; }
        public string Title { get; set; }
        public int NumberOfPositions { get; set; }
        public string ApprenticeshipTitle { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
        public List<Address>? OtherAddresses { get; set; }
        public List<Address>? EmploymentLocations { get; set; }
        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmploymentLocationOption { get; set; }
        public string? EmployerName { get; set; }
        public long? Ukprn { get; set; }
        public string? ProviderName { get; set; }

        public DateTime PostedDate { get; set; }
        public int? StandardLarsCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Route { get; set; }
        public int Level { get; set; }
        public Wage? Wage { get; set; }
        public string LongDescription { get; set; }
        public string OutcomeDescription { get; set; }
        public string TrainingDescription { get; set; }
        public IEnumerable<string> Skills { get; set; } = null!;
        public IEnumerable<Qualification> Qualifications { get; set; } = null!;
        public string? ThingsToConsider { get; set; }
        public string Id { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public string? AnonymousEmployerName { get; set; }
        public bool IsDisabilityConfident { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public bool IsRecruitVacancy { get; set; }
        public string VacancyLocationType { get; set; }
        public string? EmployerDescription { get; set; }
        public string? EmployerWebsiteUrl { get; set; }
        public string? EmployerContactPhone { get; set; }
        public string? EmployerContactEmail { get; set; }
        public string? EmployerContactName { get; set; }
        public string? ProviderContactEmail { get; set; }
        public string? ProviderContactName { get; set; }
        public string? ProviderContactPhone { get; set; }
        public string ApplicationMethod { get; set; }
        public string? ApplicationUrl { get; set; }
        public string TypicalJobTitles { get; set; }
        public string? AdditionalQuestion1 { get; set; }
        public string? AdditionalQuestion2 { get; set; }
        public string? OwnerType { get; set; }
        public string? AdditionalTrainingDescription { get; set; }
        public string? SearchTags { get; set; }
        public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    }

    public class Wage
    {
        public int Duration { get; set; }
        public DurationUnit? DurationUnit { get; set; }
        public decimal? FixedWageYearlyAmount { get; set; }
        public string? WageAdditionalInformation { get; set; }
        public WageType? WageType { get; set; }
        public decimal WeeklyHours { get; set; }
        public string? WorkingWeekDescription { get; set; }
        public decimal? ApprenticeMinimumWage { get; set; }
        public decimal? Under18NationalMinimumWage { get; set; }
        public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
        public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
        public decimal? Over25NationalMinimumWage { get; set; }
        public string WageText { get; set; } = null!;
        public string? CompanyBenefitsInformation { get; set; }

        public static implicit operator Wage(Application.Shared.Wage source)
        {
            return new Wage
            {
                Duration = source.Duration,
                DurationUnit = source.DurationUnit,
                FixedWageYearlyAmount = source.FixedWageYearlyAmount,
                WageAdditionalInformation = source.WageAdditionalInformation,
                WageType = source.WageType,
                WeeklyHours = source.WeeklyHours,
                WorkingWeekDescription = source.WorkingWeekDescription,
                ApprenticeMinimumWage = source.ApprenticeMinimumWage,
                Under18NationalMinimumWage = source.Under18NationalMinimumWage,
                Between18AndUnder21NationalMinimumWage = source.Between18AndUnder21NationalMinimumWage,
                Between21AndUnder25NationalMinimumWage = source.Between21AndUnder25NationalMinimumWage,
                Over25NationalMinimumWage = source.Over25NationalMinimumWage,
                WageText = source.WageText,
                CompanyBenefitsInformation = source.CompanyBenefitsInformation
            };
        }
    }

    public class Qualification
    {
        public string? QualificationType { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public QualificationWeighting? Weighting { get; set; }
    }
}
