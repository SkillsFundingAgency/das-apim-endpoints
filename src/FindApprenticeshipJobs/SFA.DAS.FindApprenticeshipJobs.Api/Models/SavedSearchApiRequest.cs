using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.SharedOuterApi.Models;

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
        public string? UnSubscribeToken { get; set; }
        public List<Vacancy> Vacancies { get; set; } = [];

        public class Vacancy
        {
            public string? Id { get; set; }

            public string? VacancyReference { get; set; }

            public string? Title { get; set; }

            public string? EmployerName { get; set; }

            public Address? EmployerLocation { get; set; }
            public List<Address>? EmployerLocations { get; set; }
            public AvailableWhere? EmployerLocationOption { get; set; }

            public string? Wage { get; set; }

            public string? ClosingDate { get; set; }

            public string? TrainingCourse { get; set; }

            public double? Distance { get; set; }
            public string? VacancySource { get; set; }
            public string? WageType { get; set; }
            public string? WageUnit { get; set; }
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
                        Id = vacancy.Id,
                        ClosingDate = vacancy.ClosingDate,
                        Distance = vacancy.Distance,
                        EmployerName = vacancy.EmployerName,
                        Title = vacancy.Title,
                        TrainingCourse = vacancy.TrainingCourse,
                        VacancyReference = vacancy.VacancyReference,
                        Wage = vacancy.Wage,
                        EmployerLocation = vacancy.EmployerLocation,
                        EmployerLocations = vacancy.EmployerLocations,
                        EmployerLocationOption = vacancy.EmployerLocationOption,
                        VacancySource = vacancy.VacancySource,
                        WageUnit = vacancy.WageUnit,
                        WageType = vacancy.WageType
                    }).ToList()
            };
        }
    }
}
