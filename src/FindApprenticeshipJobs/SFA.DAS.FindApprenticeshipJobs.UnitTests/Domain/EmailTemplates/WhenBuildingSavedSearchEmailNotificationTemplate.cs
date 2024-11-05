using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates
{
    [TestFixture]
    public class WhenBuildingSavedSearchEmailNotificationTemplate
    {
        [Test]
        public void Then_When_Prod_Environment_Template_And_SavedSearch_Urls_Are_Set()
        {
            var actual = new EmailEnvironmentHelper("PRd");

            actual.SavedSearchEmailNotificationTemplateId.Should().Be("TBC");
            actual.SavedSearchUnSubscribeUrl.Should().Be("https://findapprenticeship.service.gov.uk/saved-searches/{search-Id}/unsubscribe");
            actual.SearchUrl.Should().Be("https://findapprenticeship.service.gov.uk/apprenticeships?sort=AgeAsc");
            actual.VacancyDetailsUrl.Should().Be("https://findapprenticeship.service.gov.uk/apprenticeship/{vacancy-reference}");
        }
        [Test]
        public void Then_When_Non_Prod_Environment_Template_And_SavedSearch_Urls_Are_Set()
        {
            var actual = new EmailEnvironmentHelper("TEST");

            actual.SavedSearchEmailNotificationTemplateId.Should().Be("TBC");
            actual.SavedSearchUnSubscribeUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/saved-searches/{search-Id}/unsubscribe");
            actual.SearchUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/apprenticeships?sort=AgeAsc");
            actual.VacancyDetailsUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/apprenticeship/{vacancy-reference}");
        }
    }
}
