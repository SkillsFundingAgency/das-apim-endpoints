using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates
{
    [TestFixture]
    public class EmailTemplateBuilderTests
    {
        [Test]
        public void GetSavedSearchSearchParams_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            const string searchTerm = "Software Developer";
            const int distance = 10;
            const string location = "London";
            List<string> categories = ["IT", "Engineering"];
            List<string> levels = ["Intermediate", "Advanced"];
            const bool disabilityConfident = true;

            const string expected = """
                                    
                                    What: Software Developer
                                    Where: London (within 10 miles)
                                    Categories: IT, Engineering
                                    Apprenticeship levels: Intermediate, Advanced
                                    Only show Disability Confident apprenticeships
                                    
                                    """;

            // Act
            var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident);

            // Assert
            result.Trim().Should().BeEquivalentTo(expected.Trim());
        }
        
        [Test]
        public void GetSavedSearchSearchParams_WhenCalled_ReturnsExpectedResult_No_Location()
        {
            // Arrange
            const string searchTerm = "Software Developer";
            const int distance = 10;
            const string location = "London";
            List<string> categories = ["IT", "Engineering"];
            List<string> levels = ["Intermediate", "Advanced"];
            const bool disabilityConfident = true;

            const string expected = """

                                    What: Software Developer
                                    Where: London (within 10 miles)
                                    Categories: IT, Engineering
                                    Apprenticeship levels: Intermediate, Advanced
                                    Only show Disability Confident apprenticeships

                                    """;

            // Act
            var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident);

            // Assert
            result.Trim().Should().BeEquivalentTo(expected.Trim());
        }

        [Test]
        public void Then_Distance_Is_One_Mile_GetSavedSearchSearchParams_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            const string searchTerm = "Software Developer";
            const int distance = 1;
            const string location = "London";
            List<string> categories = ["IT", "Engineering"];
            List<string> levels = ["Intermediate", "Advanced"];
            const bool disabilityConfident = true;

            const string expected = """

                                    What: Software Developer
                                    Where: London (within 1 mile)
                                    Categories: IT, Engineering
                                    Apprenticeship levels: Intermediate, Advanced
                                    Only show Disability Confident apprenticeships

                                    """;

            // Act
            var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident);

            // Assert
            result.Trim().Should().BeEquivalentTo(expected.Trim());
        }


        [Test]
        public void Then_The_Location_Is_Null_GetSavedSearchSearchParams_WhenCalled_ReturnsExpectedResult()
        {
            // Arrange
            const string searchTerm = "Software Developer";
            const int distance = 1;
            const string location = "";
            List<string> categories = ["IT", "Engineering"];
            List<string> levels = ["Intermediate", "Advanced"];
            const bool disabilityConfident = true;

            const string expected = """

                                    What: Software Developer
                                    Categories: IT, Engineering
                                    Apprenticeship levels: Intermediate, Advanced
                                    Only show Disability Confident apprenticeships

                                    """;

            // Act
            var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident);

            // Assert
            result.Trim().Should().BeEquivalentTo(expected.Trim());
        }

        [Test]
        public void GetSavedSearchUrl_WithValidParameters_ReturnsQueryParameters()
        {
            // Arrange
            const string searchTerm = "software developer";
            const int distance = 10;
            const string location = "London";
            List<string> categoryIds = ["1", "2", "3"];
            List<string> levelCodes = ["4", "5"];
            const bool disabilityConfident = true;

            const string expectedQueryParameters = "&searchTerm=software developer&location=London&distance=10&routeIds=1&routeIds=2&routeIds=3&levelIds=4&levelIds=5&DisabilityConfident=true";

            // Act
            var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(searchTerm, distance, location, categoryIds, levelCodes, disabilityConfident);

            // Assert
            queryParameters.Should().Be(expectedQueryParameters);
        }

        [Test]
        public void GetSavedSearchVacanciesSnippet_Should_Return_Correct_Snippet()
        {
            // Arrange
            var environmentHelper = new EmailEnvironmentHelper("test")
            {
                VacancyDetailsUrl = "https://example.com/vacancy/{vacancy-reference}"
            };

            var vacancies = new List<PostSendSavedSearchNotificationCommand.Vacancy>
            {
                new()
                {
                    Title = "Software Developer",
                    VacancyReference = "12345",
                    EmployerName = "ABC Company",
                    Address = new PostSendSavedSearchNotificationCommand.Address
                    {
                        AddressLine1 = "123 Main St",
                        Postcode = "12345"
                    },
                    Distance = 10,
                    TrainingCourse = "Software Engineering",
                    Wage = "�30,000",
                    ClosingDate = "2022-12-31"
                }
            };

            const string expectedSnippet = """

                                           #[Software Developer](https://example.com/vacancy/12345)
                                           ABC Company
                                           123 Main St, 12345

                                           * Distance: 10 miles
                                           * Training course: Software Engineering
                                           * Wage: �30,000

                                           2022-12-31

                                           ---

                                           """;

            // Act
            var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

            // Assert
            snippet.Should().Be(expectedSnippet);
        }
        
        [Test]
        public void GetSavedSearchVacanciesSnippet_Should_Return_Correct_Snippet_With_No_Location()
        {
            // Arrange
            var environmentHelper = new EmailEnvironmentHelper("test")
            {
                VacancyDetailsUrl = "https://example.com/vacancy/{vacancy-reference}"
            };

            var vacancies = new List<PostSendSavedSearchNotificationCommand.Vacancy>
            {
                new()
                {
                    Title = "Software Developer",
                    VacancyReference = "12345",
                    EmployerName = "ABC Company",
                    Address = new PostSendSavedSearchNotificationCommand.Address
                    {
                        AddressLine1 = "123 Main St",
                        Postcode = "12345"
                    },
                    Distance = 10,
                    TrainingCourse = "Software Engineering",
                    Wage = "�30,000",
                    ClosingDate = "2022-12-31"
                }
            };

            const string expectedSnippet = """

                                           #[Software Developer](https://example.com/vacancy/12345)
                                           ABC Company
                                           123 Main St, 12345

                                           * Training course: Software Engineering
                                           * Wage: �30,000

                                           2022-12-31

                                           ---

                                           """;

            // Act
            var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, false);

            // Assert
            snippet.Should().Be(expectedSnippet);
        }
        
        private static object[] _titleTestCases =
    [
        new object?[] { "Foo", new Dictionary<int, string> {{1, "Route 1"},{2,"Route 2"}}, new Dictionary<int, string> {{1, "Level 1"}, {2, "Level 2"}}, null, true, "Foo in all of England" },
        new object?[] { "Foo", new Dictionary<int, string> {{1, "Route 1"},{2,"Route 2"}}, new Dictionary<int, string> {{1, "Level 1"}, {2, "Level 2"}}, "Hull", true, "Foo in Hull" },
        new object?[] { "Foo", new Dictionary<int, string> {{1, "Route 1"},{2,"Route 2"}}, new Dictionary<int, string> {{1, "Level 1"}, {2, "Level 2"}}, null, true, "Foo in all of England" },
        new object?[] { null, new Dictionary<int, string> {{1, "Route 1"},{2,"Route 2"},{3,"Route 3"}}, new Dictionary<int, string> {{1, "Level 1"}, {2, "Level 2"}}, null, true, "3 categories in all of England" },
        new object?[] { null, new Dictionary<int, string> {{2,"Route Two"}}, new Dictionary<int, string> {{1, "Level 1"}, {2, "Level 2"}}, null, true, "Route Two in all of England" },
        new object?[] { null, null, new Dictionary<int, string> {{1, "Level 1"},{2,"Level 2"},{3,"Level 3"},{4,"Level 4"}}, null, true, "4 apprenticeship levels in all of England" },
        new object?[] { null, null, new Dictionary<int, string> {{4, "Level 4"}}, null, true, "Level 4 in all of England" },
        new object?[] { null, null, null, null, true, "Disability Confident in all of England" },
        new object?[] { null, null, null, null, false, "All apprenticeships in all of England" },
        new object?[] { null, null, null, "Hull", false, "All apprenticeships in Hull" },
    ];
    
    [TestCaseSource(nameof(_titleTestCases))]
    public void Then_The_Title_Is_Constructed_Correctly(string? searchTerm, Dictionary<int,string>? routes, Dictionary<int,string>? levels, string? location, bool disabilityConfident, string? expectedTitle)
    {
        // act
        var command = new PostSendSavedSearchNotificationCommand()
        {
            SearchTerm = searchTerm,
            Location = location,
            DisabilityConfident = disabilityConfident,
            Levels = levels?.Select(c=>new PostSendSavedSearchNotificationCommand.Level{Code = c.Key, Name = c.Value} ).ToList(),
            Categories = routes?.Select(c=>new PostSendSavedSearchNotificationCommand.Category{Id = c.Key, Name = c.Value} ).ToList()
        };
        
        // act
        var result = EmailTemplateBuilder.BuildSearchAlertDescriptor(command);

        // assert
        result.Should().Be(expectedTitle);
    }
    }
}
