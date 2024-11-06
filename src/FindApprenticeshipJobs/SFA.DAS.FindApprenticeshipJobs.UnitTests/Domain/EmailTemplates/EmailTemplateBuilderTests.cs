using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates
{
    [TestFixture]
    public class EmailTemplateBuilderTests
    {
        [Test]
        [MoqAutoData]
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
                new PostSendSavedSearchNotificationCommand.Vacancy
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
                    Wage = "£30,000",
                    ClosingDate = "2022-12-31"
                }
            };

            const string expectedSnippet = """

                                           #[Software Developer](https://example.com/vacancy/12345)
                                           ABC Company
                                           123 Main St, 12345

                                           * Distance: 10 miles
                                           * Training course: Software Engineering
                                           * Annual wage: £30,000

                                           2022-12-31

                                           ---

                                           """;

            // Act
            var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies);

            // Assert
            snippet.Should().Be(expectedSnippet);
        }
    }
}
