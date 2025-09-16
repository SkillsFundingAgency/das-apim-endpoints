using System.Globalization;
using SFA.DAS.FindApprenticeshipJobs.Domain.Extensions;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.VacancyServices.Wage;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

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
    public List<ApprenticeshipTypes>? ApprenticeshipTypes { get; set; } = [];
    
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
        public string? StartDate { get; set; }
        public string? TrainingCourse { get; set; }
        public decimal? Distance { get; set; }
        public string? VacancySource { get; set; }
        public ApprenticeshipTypes? ApprenticeshipType { get; set; }

        public static implicit operator ApprenticeshipVacancy(GetVacanciesListItem source)
        {
            return new ApprenticeshipVacancy
            {
                Address = source.Address,
                ApprenticeshipType = source.ApprenticeshipType,
                ClosingDate = DateTimeExtension.GetClosingDate(source.ClosingDate, !string.IsNullOrEmpty(source.ApplicationUrl)),
                Distance = source.Distance.HasValue ? Math.Round(source.Distance.Value, 1) : null,
                EmployerName = source.EmployerName,
                EmploymentLocationInformation = source.EmploymentLocationInformation,
                EmploymentLocationOption = source.EmploymentLocationOption,
                Id = source.Id,
                OtherAddresses = source.OtherAddresses,
                StartDate = source.StartDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture),
                Title = source.Title,
                TrainingCourse = $"{source.CourseTitle} (level {source.CourseLevel})",
                VacancyReference = source.VacancyReference,
                VacancySource = source.VacancySource,
                Wage = source.WageText,
                WageType = ((WageType)source.WageType).ToString(),
                WageUnit = ((WageUnit)source.WageUnit).ToString(),
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