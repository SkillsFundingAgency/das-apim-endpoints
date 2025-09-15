using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;
using AvailableWhere = SFA.DAS.FindApprenticeshipJobs.Application.Shared.AvailableWhere;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates;

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
        const bool excludeNational = false;
        List<ApprenticeshipTypes> apprenticeshipTypes = [ApprenticeshipTypes.Standard, ApprenticeshipTypes.Foundation];

        const string expected = """

                                What: Software Developer
                                Where: London (within 10 miles)
                                Job category: IT, Engineering
                                Apprenticeship type: Apprenticeships, Foundation apprenticeships
                                Apprenticeship level: Intermediate, Advanced
                                Only show Disability Confident apprenticeships

                                """;

        // Act
        var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident, excludeNational, apprenticeshipTypes);

        // Assert
        result.Trim().Should().BeEquivalentTo(expected.Trim());
    }

     [Test]
     public void GetSavedSearchSearchParams_When_ExcludeNational_True_ReturnsExpectedResult()
     {
         // Arrange
         const string searchTerm = "Software Developer";
         const int distance = 10;
         const string location = "London";
         List<string> categories = ["IT", "Engineering"];
         List<string> levels = ["Intermediate", "Advanced"];
         const bool disabilityConfident = true;
         const bool excludeNational = true;
         List<ApprenticeshipTypes> apprenticeshipTypes = [ApprenticeshipTypes.Standard, ApprenticeshipTypes.Foundation];

         const string expected = """

                                 What: Software Developer
                                 Where: London (within 10 miles) - hide companies recruiting nationally
                                 Job category: IT, Engineering
                                 Apprenticeship type: Apprenticeships, Foundation apprenticeships
                                 Apprenticeship level: Intermediate, Advanced
                                 Only show Disability Confident apprenticeships

                                 """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident, excludeNational, apprenticeshipTypes);

         // Assert
         result.Trim().Should().BeEquivalentTo(expected.Trim());
     }

     [Test]
     public void GetSavedSearchSearchParams_WhenCalled_ReturnsExpectedResult_Across_All_Of_England()
     {
         // Arrange
         const string searchTerm = "Software Developer";
         const string location = "London";
         List<string> categories = ["IT", "Engineering"];
         List<string> levels = ["Intermediate", "Advanced"];
         const bool disabilityConfident = true;
         const bool excludeNational = false;
         List<ApprenticeshipTypes> apprenticeshipTypes = [ApprenticeshipTypes.Standard, ApprenticeshipTypes.Foundation];

         const string expected = """

                                 What: Software Developer
                                 Where: London (Across England)
                                 Job category: IT, Engineering
                                 Apprenticeship type: Apprenticeships, Foundation apprenticeships
                                 Apprenticeship level: Intermediate, Advanced
                                 Only show Disability Confident apprenticeships

                                 """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, null, location, categories, levels, disabilityConfident, excludeNational, apprenticeshipTypes);

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
         const bool excludeNational = false;
         List<ApprenticeshipTypes> apprenticeshipTypes = [ApprenticeshipTypes.Standard, ApprenticeshipTypes.Foundation];

         const string expected = """

                                 What: Software Developer
                                 Where: London (within 10 miles)
                                 Job category: IT, Engineering
                                 Apprenticeship type: Apprenticeships, Foundation apprenticeships
                                 Apprenticeship level: Intermediate, Advanced
                                 Only show Disability Confident apprenticeships

                                 """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident, excludeNational, apprenticeshipTypes);

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
         const bool excludeNational = false;

         const string expected = """

                                 What: Software Developer
                                 Where: London (within 1 mile)
                                 Job category: IT, Engineering
                                 Apprenticeship level: Intermediate, Advanced
                                 Only show Disability Confident apprenticeships

                                 """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams(searchTerm, distance, location, categories, levels, disabilityConfident, excludeNational, null);

         // Assert
         result.Trim().Should().BeEquivalentTo(expected.Trim());
     }

     [TestCase(null, null, "All of England")]
     [TestCase(null, 1, "All of England")]
     [TestCase(null, 10, "All of England")]
     [TestCase("", null, "All of England")]
     [TestCase("", 1, "All of England")]
     [TestCase("", 10, "All of England")]
     [TestCase("Hull", null, "Hull (Across England)")]
     [TestCase("Hull", 1, $"Hull (within 1 mile)")]
     [TestCase("Hull", 10, "Hull (within 10 miles)")]
     public void Then_The_Location_Is_Output_Correctly(string? location, int? distance, string expectedLocation)
     {
         // Arrange
         var expected = $"""
                         What: Software Developer
                         Where: {expectedLocation}
                         Job category: IT, Engineering
                         Apprenticeship level: Intermediate, Advanced
                         Only show Disability Confident apprenticeships
                         """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams("Software Developer", distance, location, ["IT", "Engineering"], ["Intermediate", "Advanced"], true, false, null);

         // Assert
         result.Trim().Should().BeEquivalentTo(expected);
     }
     
     [TestCase(ApprenticeshipTypes.Standard, "Apprenticeships")]
     [TestCase(ApprenticeshipTypes.Foundation, "Foundation apprenticeships")]
     public void Then_The_ApprenticeshipType_Is_Output_Correctly(ApprenticeshipTypes apprenticeshipType, string expectedText)
     {
         // Arrange
         var expected = $"""
                         What: Software Developer
                         Where: All of England
                         Job category: IT, Engineering
                         Apprenticeship type: {expectedText}
                         Apprenticeship level: Intermediate, Advanced
                         Only show Disability Confident apprenticeships
                         """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams("Software Developer", null, null, ["IT", "Engineering"], ["Intermediate", "Advanced"], true, false, [apprenticeshipType]);

         // Assert
         result.Trim().Should().BeEquivalentTo(expected);
     }
     
     [Test]
     public void Then_The_ApprenticeshipType_Is_Not_Displayed()
     {
         // Arrange
         const string expected = """
                         What: Software Developer
                         Where: All of England
                         Job category: IT, Engineering
                         Apprenticeship level: Intermediate, Advanced
                         Only show Disability Confident apprenticeships
                         """;

         // Act
         var result = EmailTemplateBuilder.GetSavedSearchSearchParams("Software Developer", null, null, ["IT", "Engineering"], ["Intermediate", "Advanced"], true, false, null);

         // Assert
         result.Trim().Should().BeEquivalentTo(expected);
     }

     [Test]
     public void GetSavedSearchUrl_WithValidParameters_ReturnsQueryParameters()
     {
         // arrange
         const string searchTerm = "software developer";
         const int distance = 10;
         const string location = "Greater London";
         List<string> categoryIds = ["1", "2", "3"];
         List<string> levelCodes = ["4", "5"];
         const bool disabilityConfident = true;
         const bool excludeNational = true;
         List<ApprenticeshipTypes> apprenticeshipTypes = [ApprenticeshipTypes.Standard, ApprenticeshipTypes.Foundation];

         // act
         var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(searchTerm, distance, location, categoryIds, levelCodes, disabilityConfident, excludeNational, apprenticeshipTypes);
         var qs = QueryHelpers.ParseQuery(queryParameters);
             
         // assert
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("sort", "AgeAsc"));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("searchTerm", searchTerm));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("distance", "10"));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("location", location));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("routeIds", categoryIds.ToArray()));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("levelIds", levelCodes.ToArray()));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("apprenticeshipTypes", apprenticeshipTypes.Select(x => $"{x}").ToArray()));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("DisabilityConfident", "true"));
         qs.Should().ContainEquivalentOf(new KeyValuePair<string, StringValues>("excludeNational", "true"));
     }
     
     [Test]
     public void GetSavedSearchUrl_Ignores_Zero_Distance()
     {
         // arrange
         const string searchTerm = "software developer";
         const int distance = 0;
         const string location = "Greater London";

         // act
         var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(searchTerm, distance, location, null, null, null, null, null);

         // assert
         queryParameters.Should().NotContain("&distance");
     }
     
     [Test]
     public void GetSavedSearchUrl_Ignores_No_Categories()
     {
         // arrange
         const string searchTerm = "software developer";

         // act
         var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(searchTerm, null, null, [], null, null, null, null);

         // assert
         queryParameters.Should().NotContain("routeIds");
     }
     
     [Test]
     public void GetSavedSearchUrl_Ignores_No_Levels()
     {
         // arrange
         const string searchTerm = "software developer";

         // act
         var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(searchTerm, null, null, null, [], null, null, null);

         // assert
         queryParameters.Should().NotContain("levelIds");
     }
     
     [Test]
     public void GetSavedSearchUrl_Ignores_No_ApprenticeshipTypes()
     {
         // arrange
         const string searchTerm = "software developer";

         // act
         var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(searchTerm, null, null, null, null, null, null, []);

         // assert
         queryParameters.Should().NotContain("apprenticeshipTypes");
     }

    [Test]
    public void GetSavedSearchVacanciesSnippet_Then_Source_RAA_Should_Return_Correct_Snippet()
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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                Distance = 10,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31",
                StartDate = "2025-03-01",
                VacancySource = nameof(VacancyDataSource.Raa)
            }
        };

        const string expectedSnippet = """

                                       #[Software Developer](https://example.com/vacancy/12345)
                                       ABC Company
                                       123 Main St (12345)

                                       * Distance: 10 miles
                                       * Start date: 2025-03-01
                                       * Training course: Software Engineering
                                       * Wage: £30,000 a year

                                       2022-12-31

                                       ---

                                       """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }

    [Test]
    public void GetSavedSearchVacanciesSnippet_Then_Source_NHS_Should_Return_Correct_Snippet()
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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                Distance = 10,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31",
                StartDate = "0001-01-01",
                VacancySource = nameof(VacancyDataSource.Nhs)
            }
        };

        const string expectedSnippet = """

                                       #[Software Developer (from NHS Jobs)](https://example.com/vacancy/12345)
                                       ABC Company
                                       123 Main St (12345)

                                       * Distance: 10 miles
                                       * Training course: See more details on NHS Jobs
                                       * Wage: £30,000 a year

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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                Distance = 10,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31"
            }
        };

        const string expectedSnippet = """

                                       #[Software Developer](https://example.com/vacancy/12345)
                                       ABC Company
                                       123 Main St (12345)

                                       * Training course: Software Engineering
                                       * Wage: £30,000 a year

                                       2022-12-31

                                       ---

                                       """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, false);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }

    [Test]
    public void GetSavedSearchVacanciesSnippet_Should_Return_Correct_Snippet_With_One_Location()
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
                EmploymentLocationOption = AvailableWhere.OneLocation,
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                OtherAddresses =
                [
                    new Address
                    {
                        AddressLine1 = "123 Main St",
                        Postcode = "12345"
                    }
                ],
                Distance = 10,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31"
            }
        };

        const string expectedSnippet = """

                                       #[Software Developer](https://example.com/vacancy/12345)
                                       ABC Company
                                       123 Main St (12345)

                                       * Training course: Software Engineering
                                       * Wage: £30,000 a year

                                       2022-12-31

                                       ---

                                       """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, false);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }

    [TestCase("Competitive", "", "Competitive")]
    [TestCase("", "month", "£30,000 a year")]
    [TestCase("", "hour", "£30,000 a year")]
    public void GetSavedSearchVacanciesSnippet_Should_Return_Snippet_With_Correct_Wage_Text_For_Faa_VacancySource_And_Different_WageTypes(
        string wagetype, string wageUnit, string expectedWageText)
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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                Distance = 1,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31",
                VacancySource = "FAA",
                WageUnit = wageUnit,
                WageType = wagetype
            }
        };

        var expectedSnippet = $"""

                               #[Software Developer](https://example.com/vacancy/12345)
                               ABC Company
                               123 Main St (12345)

                               * Distance: 1 mile
                               * Training course: Software Engineering
                               * Wage: {expectedWageText}

                               2022-12-31

                               ---

                               """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }

    [TestCase("Competitive", "", "Competitive")]
    [TestCase("", "month", "£30,000 a year")]
    [TestCase("", "hour", "£30,000 a year")]
    public void GetSavedSearchVacanciesSnippet_Should_Return_Snippet_With_Correct_Wage_Text_For_Faa_VacancySource_And_Different_WageTypes_With_OneLocation(
        string wagetype, string wageUnit, string expectedWageText)
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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                OtherAddresses =
                [
                    new Address
                    {
                        AddressLine1 = "123 Main St",
                        Postcode = "12345"
                    }
                ],
                Distance = 1,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31",
                VacancySource = "FAA",
                WageUnit = wageUnit,
                WageType = wagetype
            }
        };

        var expectedSnippet = $"""

                               #[Software Developer](https://example.com/vacancy/12345)
                               ABC Company
                               123 Main St (12345)

                               * Distance: 1 mile
                               * Training course: Software Engineering
                               * Wage: {expectedWageText}

                               2022-12-31

                               ---

                               """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }

    [TestCase("CompetitiveSalary", "Annually", "Negotiable", "Negotiable")]
    [TestCase("FixedWage", "Annually", "£10296.00", "£10,296 a year")]
    [TestCase("FixedWage", "Annually", "£23615.00 to £23615.00", "£23,615 to £23,615 a year")]
    [TestCase("FixedWage", "Annually", "£6.40", "£6.40 an hour")]
    public void GetSavedSearchVacanciesSnippet_Should_Return_Snippet_With_Correct_Wage_Text_For_Nhs_VacancySource_And_Different_WageTypes(
        string wagetype, string wageUnit, string wage, string expectedWageText)
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
                Title = "Mental Health Nurse",
                VacancyReference = "12345",
                EmployerName = "NHS Jobs",
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                Distance = 10,
                TrainingCourse = " (level 0)",
                Wage = wage,
                ClosingDate = "2022-12-31",
                VacancySource = "NHS",
                WageUnit = wageUnit,
                WageType = wagetype
            }
        };

        var expectedSnippet = $"""

                               #[Mental Health Nurse (from NHS Jobs)](https://example.com/vacancy/12345)
                               NHS Jobs
                               123 Main St (12345)

                               * Distance: 10 miles
                               * Training course: See more details on NHS Jobs
                               * Wage: {expectedWageText}

                               2022-12-31

                               ---

                               """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

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
    public void Then_The_Title_Is_Constructed_Correctly(string? searchTerm, Dictionary<int, string>? routes, Dictionary<int, string>? levels, string? location, bool disabilityConfident, string? expectedTitle)
    {
        // act
        var command = new PostSendSavedSearchNotificationCommand()
        {
            SearchTerm = searchTerm,
            Location = location,
            DisabilityConfident = disabilityConfident,
            Levels = levels?.Select(c => new PostSendSavedSearchNotificationCommand.Level { Code = c.Key, Name = c.Value }).ToList(),
            Categories = routes?.Select(c => new PostSendSavedSearchNotificationCommand.Category { Id = c.Key, Name = c.Value }).ToList()
        };

        // act
        var result = EmailTemplateBuilder.BuildSearchAlertDescriptor(command);

        // assert
        result.Should().Be(expectedTitle);
    }

    [Test]
    public void GetSavedSearchVacanciesSnippet_Should_Return_Correct_Snippet_Multiple_Locations()
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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                EmploymentLocationOption = AvailableWhere.MultipleLocations,
                OtherAddresses =
                [
                    new Address
                    {
                        AddressLine1 = "address 1",
                        Postcode = "postcode 1"
                    },

                    new Address
                    {
                        AddressLine1 = "address 2",
                        Postcode = "postcode 2"
                    },

                    new Address
                    {
                        AddressLine1 = "address 3",
                        Postcode = "postcode 4"
                    }
                ],
                Distance = 10,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31",
                VacancySource = "FAA"
            }
        };

        const string expectedSnippet = """

                                       #[Software Developer](https://example.com/vacancy/12345)
                                       ABC Company
                                       123 Main St (12345) and 3 other available locations

                                       * Distance: 10 miles
                                       * Training course: Software Engineering
                                       * Wage: £30,000 a year

                                       2022-12-31

                                       ---

                                       """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }

    [Test]
    public void GetSavedSearchVacanciesSnippet_Should_Return_Correct_Snippet_Recruit_Nationally()
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
                EmployerLocation = new Address
                {
                    AddressLine1 = "123 Main St",
                    Postcode = "12345"
                },
                EmploymentLocationOption = AvailableWhere.AcrossEngland,
                OtherAddresses =
                [
                    new Address
                    {
                        AddressLine1 = "123 Main St",
                        Postcode = "12345"
                    },
                    new Address
                    {
                        AddressLine1 = "address 1",
                        Postcode = "postcode 1"
                    },

                    new Address
                    {
                        AddressLine1 = "address 2",
                        Postcode = "postcode 2"
                    },

                    new Address
                    {
                        AddressLine1 = "address 3",
                        Postcode = "postcode 4"
                    }
                ],
                Distance = 10,
                TrainingCourse = "Software Engineering",
                Wage = "£30,000 a year",
                ClosingDate = "2022-12-31",
                VacancySource = "FAA"
            }
        };

        const string expectedSnippet = """

                                       #[Software Developer](https://example.com/vacancy/12345)
                                       ABC Company
                                       Recruiting nationally

                                       * Distance: 10 miles
                                       * Training course: Software Engineering
                                       * Wage: £30,000 a year

                                       2022-12-31

                                       ---

                                       """;

        // Act
        var snippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(environmentHelper, vacancies, true);

        // Assert
        snippet.Should().Be(expectedSnippet);
    }
}