using SFA.DAS.Recruit.Domain.EmailTemplates;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Domain.EmailTemplates;
[TestFixture]
internal class WhenBuildingSharedApplicationReviewedEmailTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,
        string recipientEmail,
        string advertTitle,
        string firstName,
        string vacancyReference,
        string employerName,
        string manageVacancyUrl,
        string notificationSettingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"VACcode", vacancyReference },
            {"employer", employerName },
            {"vacancyTitle", advertTitle },
            {"manageVacancyURL", manageVacancyUrl },
            {"notificationSettingsURL", notificationSettingsUrl }
        };

        var actual = new SharedApplicationsReturnedEmailTemplate(templateId,
            recipientEmail,
            advertTitle,
            firstName,
            vacancyReference,
            employerName,
            manageVacancyUrl,
            notificationSettingsUrl);

        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}
