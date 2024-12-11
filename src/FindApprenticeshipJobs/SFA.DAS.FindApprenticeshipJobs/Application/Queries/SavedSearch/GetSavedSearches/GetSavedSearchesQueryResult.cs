using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetSavedSearches
{
    public record GetSavedSearchesQueryResult
    {
        public List<SearchResult> SavedSearchResults { get; set; } = null!;
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public class SearchResult
        {
            public Guid Id { get; set; }
            public UserDetails? User { get; set; }
            public List<Category>? Categories { get; set; } = [];
            public List<Level>? Levels { get; set; } = [];
            public decimal? Distance { get; set; }
            public string? SearchTerm { get; set; }
            public string? Location { get; set; }
            public bool DisabilityConfident { get; set; }
            public string? UnSubscribeToken { get; set; }
            public List<ApprenticeshipVacancy> Vacancies { get; set; } = [];
            public class ApprenticeshipVacancy
            {
                public string? Id { get; set; }
                public string? VacancyReference { get; set; }
                public string? Title { get; set; }
                public string? EmployerName { get; set; }
                public Address Address { get; set; } = null!;
                public string? Wage { get; set; }
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
                        ClosingDate = GetClosingDate(source.ClosingDate, !string.IsNullOrEmpty(source.ApplicationUrl)),
                        Title = source.Title,
                        EmployerName = source.EmployerName,
                        Wage = source.WageText,
                        Address = source.VacancyAddress,
                        TrainingCourse = $"{source.CourseTitle} (level {source.CourseLevel})",
                        Distance = source.Distance.HasValue ? Math.Round(source.Distance.Value, 1) : null,
                        VacancySource = source.VacancySource,
                    };
                }
            }
            public class Address
            {
                public string? AddressLine1 { get; set; }
                public string? AddressLine2 { get; set; }
                public string? AddressLine3 { get; set; }
                public string? AddressLine4 { get; set; }
                public string? Postcode { get; set; }

                public static implicit operator Address(VacancyAddress source)
                {
                    return new Address
                    {
                        AddressLine1 = source.AddressLine1,
                        AddressLine2 = source.AddressLine2,
                        AddressLine3 = source.AddressLine3,
                        AddressLine4 = source.AddressLine4,
                        Postcode = source.Postcode,
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

            public static implicit operator SearchResult(GetVacanciesResponse source)
            {
                return new SearchResult
                {
                    Vacancies = source.ApprenticeshipVacancies.Select(x => (ApprenticeshipVacancy)x).ToList()
                };
            }

            private static string GetClosingDate(DateTime closingDate, bool isExternalVacancy = false)
            {
                var timeSuffix = isExternalVacancy ? string.Empty : " at 11:59pm";
                var timeUntilClosing = closingDate.Date - DateTime.UtcNow;
                var daysToExpiry = (int)Math.Ceiling(timeUntilClosing.TotalDays);

                return daysToExpiry switch
                {
                    < 0 => $"Closed on {closingDate:dddd d MMMM}",
                    0 => $"Closes today{timeSuffix}",
                    1 => $"Closes tomorrow ({closingDate:dddd d MMMM}{timeSuffix})",
                    <= 31 => $"Closes in {daysToExpiry} days ({closingDate:dddd d MMMM}{timeSuffix})",
                    _ => $"Closes on {closingDate:dddd d MMMM}"
                };
            }
        }
    }
}