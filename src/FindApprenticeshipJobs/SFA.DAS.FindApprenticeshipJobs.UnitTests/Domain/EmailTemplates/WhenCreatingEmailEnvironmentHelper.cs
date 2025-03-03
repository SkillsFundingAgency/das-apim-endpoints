using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates;

public class WhenCreatingEmailEnvironmentHelper
{
    [Test]
    public void Then_When_Prod_Environment_Template_And_ApplicationUrl_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("PRd");

        actual.ApplicationReminderEmailTemplateId.Should().Be("a0248572-0d33-46a3-8bb8-560a95cc6e69");
        actual.VacancyClosedEarlyTemplateId.Should().Be("47c904f5-685c-429c-bf3a-1cf23205740c");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
        actual.VacancyUrl.Should().Be("https://findapprenticeship.service.gov.uk/vacancies");
        actual.SettingsUrl.Should().Be("https://findapprenticeship.service.gov.uk/settings");
    }
    [Test]
    public void Then_When_Non_Prod_Environment_Template_And_ApplicationUrl_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("TEST");

        actual.ApplicationReminderEmailTemplateId.Should().Be("00d36062-dbba-47b7-8442-10f80f42e127");
        actual.VacancyClosedEarlyTemplateId.Should().Be("217c85c1-1fbf-4f61-ae84-e161a01aa791");
        actual.CandidateApplicationUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/applications");
        actual.VacancyUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/vacancies");
        actual.SettingsUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/settings");
    }
}