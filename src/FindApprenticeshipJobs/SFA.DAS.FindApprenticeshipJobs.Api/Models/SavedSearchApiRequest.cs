using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Models
{
    public class SavedSearchApiRequest
    {
        public Guid Id { get; set; }
        public UserDetails User { get; set; } = new();
        public List<Category>? Categories { get; set; } = [];
        public List<Level>? Levels { get; set; } = [];
        public string? Location { get; set; }
        public decimal? Distance { get; set; }
        public string? SearchTerm { get; set; }
        public bool DisabilityConfident { get; set; }
        public bool? ExcludeNational { get; set; }
        public string? UnSubscribeToken { get; set; }
        public List<Vacancy> Vacancies { get; set; } = [];
        public List<ApprenticeshipTypes>? ApprenticeshipTypes { get; set; } = [];

        public class Vacancy
        {
            public string? Id { get; set; }

            public string? VacancyReference { get; set; }

            public string? Title { get; set; }

            public string? EmployerName { get; set; }

            public Address? Address { get; set; }
            public List<Address>? OtherAddresses { get; set; }
            public AvailableWhere? EmploymentLocationOption { get; set; }

            public string? Wage { get; set; }

            public string? ClosingDate { get; set; }
            public string? StartDate { get; set; }

            public string? TrainingCourse { get; set; }

            public double? Distance { get; set; }
            public string? VacancySource { get; set; }
            public string? WageType { get; set; }
            public string? WageUnit { get; set; }
            public ApprenticeshipTypes? ApprenticeshipType { get; set; }
        }
        
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
        public class UserDetails
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string? MiddleNames { get; set; }
            public string? LastName { get; set; }
            public string Email { get; set; } = null!;
        }

        public PostSendSavedSearchNotificationCommand MapToCommand(SavedSearchApiRequest savedSearchApiRequest)
        {
            return new PostSendSavedSearchNotificationCommand
            {
                ApprenticeshipTypes = savedSearchApiRequest.ApprenticeshipTypes,
                Id = savedSearchApiRequest.Id,
                Categories = savedSearchApiRequest.Categories?.Select(category =>
                    new PostSendSavedSearchNotificationCommand.Category
                    {
                        Id = category.Id,
                        Name = category.Name
                    }).ToList(),
                Levels = savedSearchApiRequest.Levels?.Select(level => new PostSendSavedSearchNotificationCommand.Level
                {
                    Code = level.Code,
                    Name = level.Name
                }).ToList(),
                Distance = savedSearchApiRequest.Distance,
                DisabilityConfident = savedSearchApiRequest.DisabilityConfident,
                ExcludeNational = savedSearchApiRequest.ExcludeNational,
                Location = savedSearchApiRequest.Location,
                SearchTerm = savedSearchApiRequest.SearchTerm,
                UnSubscribeToken = savedSearchApiRequest.UnSubscribeToken,
                User = new PostSendSavedSearchNotificationCommand.UserDetails
                {
                    Id = savedSearchApiRequest.User.Id,
                    Email = savedSearchApiRequest.User.Email,
                    FirstName = savedSearchApiRequest.User.FirstName,
                    MiddleNames = savedSearchApiRequest.User.MiddleNames,
                    LastName = savedSearchApiRequest.User.LastName
                },
                Vacancies = savedSearchApiRequest.Vacancies.Select(vacancy =>
                    new PostSendSavedSearchNotificationCommand.Vacancy
                    {
                        ApprenticeshipType = vacancy.ApprenticeshipType,
                        ClosingDate = vacancy.ClosingDate,
                        Distance = vacancy.Distance,
                        EmployerLocation = vacancy.Address,
                        EmployerName = vacancy.EmployerName,
                        EmploymentLocationOption = vacancy.EmploymentLocationOption,
                        Id = vacancy.Id,
                        OtherAddresses = vacancy.OtherAddresses,
                        StartDate = vacancy.StartDate,
                        Title = vacancy.Title,
                        TrainingCourse = vacancy.TrainingCourse,
                        VacancyReference = vacancy.VacancyReference,
                        VacancySource = vacancy.VacancySource,
                        Wage = vacancy.Wage,
                        WageType = vacancy.WageType,
                        WageUnit = vacancy.WageUnit,
                    }).ToList()
            };
        }
    }
}
