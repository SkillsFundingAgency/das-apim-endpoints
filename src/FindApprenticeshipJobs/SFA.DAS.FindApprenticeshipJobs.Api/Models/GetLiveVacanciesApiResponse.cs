using SFA.DAS.FindApprenticeshipJobs.Application.Queries;
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
                Address = source.Address == null ? null : (Address)source.Address,
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
                IsRecruitVacancy = true,
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
                AccountPublicHashedId = source.AccountPublicHashedId,
                AccountLegalEntityPublicHashedId = source.AccountLegalEntityPublicHashedId,
                ApplicationMethod = source.ApplicationMethod,
                ApplicationUrl = source.ApplicationUrl,
                TypicalJobTitles = source.TypicalJobTitles,
                AdditionalQuestion1 = source.AdditionalQuestion1,
                AdditionalQuestion2 = source.AdditionalQuestion2,
            };
        }

        public int RouteCode { get; set; }
        public string? DurationUnit { get; set; }
        public int Duration { get; set; }
        public string? AccountLegalEntityPublicHashedId { get; set; }
        public string? AccountPublicHashedId { get; set; }
        public string ApprenticeshipLevel { get; set; }
        public Guid VacancyId { get; set; }
        public long VacancyReference { get; set; }
        public string Title { get; set; }
        public int NumberOfPositions { get; set; }
        public string ApprenticeshipTitle { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
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

    }

    public class Address
    {
        public static implicit operator Address(Application.Shared.Address source)
        {
            return new Address
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode,
                Latitude = source.Latitude,
                Longitude = source.Longitude
            };
        }

        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? AddressLine4 { get; set; }
        public string? Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Wage
    {
        public int Duration { get; set; }
        public string? DurationUnit { get; set; }
        public decimal? FixedWageYearlyAmount { get; set; }
        public string? WageAdditionalInformation { get; set; }
        public string? WageType { get; set; }
        public decimal WeeklyHours { get; set; }
        public string? WorkingWeekDescription { get; set; }
        public decimal? ApprenticeMinimumWage { get; set; }
        public decimal? Under18NationalMinimumWage { get; set; }
        public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
        public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
        public decimal? Over25NationalMinimumWage { get; set; }
        public string WageText { get; set; } = null!;

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
                WageText = source.WageText
            };
        }
    }

    public class Qualification
    {
        public string? QualificationType { get; set; }
        public string? Subject { get; set; }
        public string? Grade { get; set; }
        public string? Weighting { get; set; }
    }
}
