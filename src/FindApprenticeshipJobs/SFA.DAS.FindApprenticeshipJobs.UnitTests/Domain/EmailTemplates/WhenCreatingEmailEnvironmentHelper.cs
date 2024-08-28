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

        actual.ApplicationReminderEmailTemplateId.Should().Be("970d86cf-a80f-4012-81e5-eff719d2f1b0");
        actual.VacancyClosedEarlyTemplateId.Should().Be("8eed4437-9b7d-422b-be3b-dd943c64e0b6");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
        actual.VacancyUrl.Should().Be("https://findapprenticeship.service.gov.uk/vacancies");
        actual.SettingsUrl.Should().Be("https://findapprenticeship.service.gov.uk/settings");
    }
    [Test]
    public void Then_When_Non_Prod_Environment_Template_And_ApplicationUrl_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("TEST");

        actual.ApplicationReminderEmailTemplateId.Should().Be("78ce88c5-bc7d-4232-86d9-46b864af23ee");
        actual.VacancyClosedEarlyTemplateId.Should().Be("41d0c41b-e95f-4dd7-b8b5-97c87ccd8141");
        actual.CandidateApplicationUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/applications");
        actual.VacancyUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/vacancies");
        actual.SettingsUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/settings");
    }
}