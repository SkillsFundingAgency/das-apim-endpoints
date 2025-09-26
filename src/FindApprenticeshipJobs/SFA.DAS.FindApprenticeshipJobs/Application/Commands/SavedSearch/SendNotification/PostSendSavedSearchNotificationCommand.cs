using MediatR;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;

public record PostSendSavedSearchNotificationCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public UserDetails User { get; set; } = null!;
    public List<Category>? Categories { get; set; } = [];
    public List<Level>? Levels { get; set; } = [];
    public decimal? Distance { get; set; }
    public string? Location { get; set; }
    public string? SearchTerm { get; set; }
    public bool DisabilityConfident { get; set; }
    public bool? ExcludeNational { get; set; }
    public string? UnSubscribeToken { get; set; }
    public List<Vacancy> Vacancies { get; set; } = [];
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

    public class Vacancy
    {
        public string? Id { get; set; }

        public string? VacancyReference { get; set; }

        public string? Title { get; set; }

        public string? EmployerName { get; set; }

        public Address? EmployerLocation { get; set; }
        public List<Address>? OtherAddresses { get; set; }
        public AvailableWhere? EmploymentLocationOption { get; set; }


        public string? Wage { get; set; }

        public string? WageUnit { get; set; }

        public string? WageType { get; set; }

        public string? ClosingDate { get; set; }
        public string? StartDate { get; set; }

        public string? TrainingCourse { get; set; }

        public double? Distance { get; set; }

        public string? VacancySource { get; set; }
        public ApprenticeshipTypes? ApprenticeshipType { get; set; }
    }
    public class UserDetails
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleNames { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
    }
}