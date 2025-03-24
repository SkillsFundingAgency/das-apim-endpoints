using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.Extensions;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.VacancyServices.Wage;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearchVacancies;

public class GetSavedSearchVacanciesQueryResult
{
    public Guid Id { get; set; }
    public UserDetails? User { get; set; }
    public List<Category>? Categories { get; set; } = [];
    public List<Level>? Levels { get; set; } = [];
    public decimal? Distance { get; set; }
    public string? SearchTerm { get; set; }
    public string? Location { get; set; }
    public bool DisabilityConfident { get; set; }
    public bool? ExcludeNational { get; set; }
    public string? UnSubscribeToken { get; set; }
    public List<ApprenticeshipVacancy> Vacancies { get; set; } = [];
    
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class Level
    {
        public int Code { get; set; }
        public string? Name { get; set; }
    }
    public class ApprenticeshipVacancy
    {
        public string? Id { get; set; }
        public string? VacancyReference { get; set; }
        public string? Title { get; set; }
        public string? EmployerName { get; set; }
        public Address Address { get; set; }
        public List<Address>? OtherAddresses { get; set; }
        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmploymentLocationOption { get; set; }
        public string? Wage { get; set; }
        public string? WageUnit { get; set; }
        public string? WageType { get; set; }
        public string? ClosingDate { get; set; }
        public string? TrainingCourse { get; set; }
        public decimal? Distance { get; set; }
        public string? VacancySource { get; set; }

        public static implicit operator ApprenticeshipVacancy(GetVacanciesListItem source)
        {
            return new ApprenticeshipVacancy
            {
                Id = source.Id,
                VacancyReference = source.VacancyReference,
                ClosingDate = DateTimeExtension.GetClosingDate(source.ClosingDate, !string.IsNullOrEmpty(source.ApplicationUrl)),
                Title = source.Title,
                EmployerName = source.EmployerName,
                Wage = source.WageText,
                WageUnit = ((WageUnit)source.WageUnit).ToString(),
                WageType = ((WageType)source.WageType).ToString(),
                Address = source.Address,
                OtherAddresses = source.OtherAddresses,
                EmploymentLocationOption = source.EmploymentLocationOption,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                TrainingCourse = $"{source.CourseTitle} (level {source.CourseLevel})",
                Distance = source.Distance.HasValue ? Math.Round(source.Distance.Value, 1) : null,
                VacancySource = source.VacancySource,
            };
        }
    }
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleNames { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        public static implicit operator UserDetails(GetCandidateApiResponse source)
        {
            return new UserDetails
            {
                Id = source.Id,
                Email = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                MiddleNames = source.MiddleNames,
            };
        }
    }
}