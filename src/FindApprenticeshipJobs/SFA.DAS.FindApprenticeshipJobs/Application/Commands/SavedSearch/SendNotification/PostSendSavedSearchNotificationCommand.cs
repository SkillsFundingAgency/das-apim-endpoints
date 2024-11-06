using MediatR;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification
{
    public record PostSendSavedSearchNotificationCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public UserDetails User { get; set; } = null!;
        public List<Category>? Categories { get; set; } = [];
        public List<Level>? Levels { get; set; } = [];
        public int Distance { get; set; }
        public string? Location { get; set; }
        public string? SearchTerm { get; set; }
        public bool DisabilityConfident { get; set; }
        public string? UnSubscribeToken { get; set; }
        public List<Vacancy> Vacancies { get; set; } = [];

        public class Category
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Level
        {
            public int Code { get; set; }
            public string Name { get; set; }
        }

        public class Vacancy
        {
            public string? Id { get; set; }

            public string? VacancyReference { get; set; }

            public string? Title { get; set; }

            public string? EmployerName { get; set; }

            public Address Address { get; set; } = new();

            public string? Wage { get; set; }

            public string? ClosingDate { get; set; }

            public string? TrainingCourse { get; set; }

            public double? Distance { get; set; }
        }

        public class Address
        {
            public string? AddressLine1 { get; set; }

            public string? AddressLine2 { get; set; }

            public string? AddressLine3 { get; set; }

            public string? AddressLine4 { get; set; }

            public string? Postcode { get; set; }
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
}
