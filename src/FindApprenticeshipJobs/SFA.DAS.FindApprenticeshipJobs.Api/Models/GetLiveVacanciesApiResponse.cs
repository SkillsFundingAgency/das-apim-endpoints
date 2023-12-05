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
                VacancyTitle = source.VacancyTitle,
                NumberOfPositions = source.NumberOfPositions,
                ApprenticeshipTitle = source.ApprenticeshipTitle,
                ProgrammeId = source.ProgrammeId,
                ProgrammeType = source.ProgrammeType,
                Route = source.Route,
                Description = source.Description,
                EmployerLocation = source.EmployerLocation == null ? null : (Address)source.EmployerLocation,
                LiveDate = source.LiveDate,
                ClosingDate = source.ClosingDate,
                StartDate = source.StartDate,
                EmployerName = source.EmployerName,
                ProviderId = source.ProviderId,
                ProviderName = source.ProviderName,
                Level = source.Level,
                Wage = source.Wage == null ? null : (Wage)source.Wage,
                OutcomeDescription = source.OutcomeDescription,
                LongDescription = source.LongDescription,
                TrainingDescription = source.TrainingDescription,
                Skills = source.Skills,
                Qualifications = source.Qualifications.Select(q => new Qualification()
                {
                    QualificationType = q.QualificationType,
                    Subject = q.Subject,
                    Grade = q.Grade,
                    Weighting = (QualificationWeighting)q.Weighting
                }).ToList(),
                ThingsToConsider = source.ThingsToConsider,
                Id = source.Id,
                IsDisabilityConfident = (DisabilityConfident)source.IsDisabilityConfident,
                IsEmployerAnonymous = source.IsEmployerAnonymous,
                EmployerDescription = source.EmployerDescription,
                EmployerWebsiteUrl = source.EmployerWebsiteUrl,
                IsRecruitVacancy = true,
                AnonymousEmployerName = source.AnonymousEmployerName,
                Category = source.Category,
                CategoryCode = source.CategoryCode,
                IsPositiveAboutDisability = source.IsPositiveAboutDisability,
                SubCategory = source.SubCategory,
                SubCategoryCode = source.SubCategoryCode,
                VacancyLocationType = source.VacancyLocationType,
                WageAmountLowerBand = source.WageAmountLowerBand,
                WageAmountUpperBand = source.WageAmountUpperBand,
                ExpectedDuration = source.ExpectedDuration,
                Distance = source.Distance,
                Score = source.Score,
                EmployerContactName = source.EmployerContactName,
                EmployerContactEmail = source.EmployerContactEmail,
                EmployerContactPhone = source.EmployerContactPhone
            };
        }

        public Guid VacancyId { get; set; }
        public long VacancyReference { get; set; }
        public string VacancyTitle { get; set; }
        public int NumberOfPositions { get; set; }
        public string ApprenticeshipTitle { get; set; }
        public string? Description { get; set; }
        public Address? EmployerLocation { get; set; }
        public string? EmployerName { get; set; }
        public long? ProviderId { get; set; }
        public string? ProviderName { get; set; }

        public DateTime LiveDate { get; set; }
        public string? ProgrammeId { get; set; }
        public string? ProgrammeType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string Route { get; set; }
        public int Level { get; set; }
        public Wage? Wage { get; set; }
        public string LongDescription { get; set; }
        public string OutcomeDescription { get; set; }
        public string TrainingDescription { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<Qualification> Qualifications { get; set; }
        public string ThingsToConsider { get; set; }
        public string Id { get; set; }
        public string AnonymousEmployerName { get; set; }
        public string Category { get; set; }
        public string CategoryCode { get; set; }
        public DisabilityConfident IsDisabilityConfident { get; set; }
        public bool IsEmployerAnonymous { get; set; }
        public bool IsPositiveAboutDisability { get; set; }
        public bool IsRecruitVacancy { get; set; }
        public string SubCategory { get; set; }
        public string SubCategoryCode { get; set; }
        public string VacancyLocationType { get; set; }
        public long WageAmountLowerBand { get; set; }
        public long WageAmountUpperBand { get; set; }
        public int ExpectedDuration { get; set; }
        public int Distance { get; set; }
        public int Score { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactName { get; set; }
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
                WorkingWeekDescription = source.WorkingWeekDescription
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

    public enum QualificationWeighting
    {
        Essential,
        Desired
    }

    public enum DisabilityConfident
    {
        No = 0,
        Yes
    }
}
