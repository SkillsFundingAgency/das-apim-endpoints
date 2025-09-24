using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates;

[TestFixture]
public class WhenBuildingSavedSearchEmailNotificationTemplate
{
    [Test]
    public void Then_When_Prod_Environment_Template_And_SavedSearch_Urls_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("PRd");

        actual.SavedSearchEmailNotificationTemplateId.Should().Be("668d5683-1252-43ac-ba57-8023a97813a3");
        actual.SavedSearchUnSubscribeUrl.Should().Be("https://findapprenticeship.service.gov.uk/saved-searches/unsubscribe?id=");
        actual.SearchUrl.Should().Be("https://findapprenticeship.service.gov.uk/apprenticeships");
        actual.VacancyDetailsUrl.Should().Be("https://findapprenticeship.service.gov.uk/apprenticeship/{vacancy-reference}");
    }
    [Test]
    public void Then_When_Non_Prod_Environment_Template_And_SavedSearch_Urls_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("TEST");

        actual.SavedSearchEmailNotificationTemplateId.Should().Be("4fa070f9-db70-4763-b3f7-04af5e08b167");
        actual.SavedSearchUnSubscribeUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/saved-searches/unsubscribe?id=");
        actual.SearchUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/apprenticeships");
        actual.VacancyDetailsUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/apprenticeship/{vacancy-reference}");
    }
}