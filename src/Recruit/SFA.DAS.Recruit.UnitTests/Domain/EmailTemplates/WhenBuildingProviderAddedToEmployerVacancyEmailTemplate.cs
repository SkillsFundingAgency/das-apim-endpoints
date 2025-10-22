using SFA.DAS.Recruit.Domain.EmailTemplates;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Domain.EmailTemplates;
[TestFixture]
internal class WhenBuildingProviderAddedToEmployerVacancyEmailTemplate
{
    [Test, MoqAutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,
        string recipientEmail,
        string firstName,
        string advertTitle,
        string vacancyReference,
        string employerName,
        string employerEmail,
        string location,
        string findAnApprenticeshipAdvertUrl,
        string courseTitle,
        string positions,
        string startDate,
        string duration,
        string notificationSettingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"advertTitle", advertTitle },
            {"VACnumber", vacancyReference },
            {"employer", employerName },
            {"location", location },
            {"applicationUrl", findAnApprenticeshipAdvertUrl },
            {"submitterEmail", employerEmail },
            {"courseTitle", courseTitle },
            {"positions", positions },
            {"startDate", startDate },
            {"duration", duration },
            {"notificationSettingsURL", notificationSettingsUrl },
        };

        var actual = new ProviderAddedToEmployerVacancyEmailTemplate(templateId,
            recipientEmail,
            firstName,
            advertTitle,
            vacancyReference,
            employerName,
            employerEmail,
            location,
            findAnApprenticeshipAdvertUrl,
            courseTitle,
            positions,
            startDate,
            duration,
            notificationSettingsUrl);

        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }

}
