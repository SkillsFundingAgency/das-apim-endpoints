using System.Collections.Generic;
using SFA.DAS.Recruit.Domain.EmailTemplates;

namespace SFA.DAS.Recruit.UnitTests.Domain.EmailTemplates;

public class WhenBuildingVacancyReviewResponseEmailTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string advertTitle, string firstName, string vacancyReference, string employerName, string findAnApprenticeshipAdvertUrl, string notificationSettingsUrl, string location)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"advertTitle", advertTitle },
            {"firstName", firstName },
            {"employerName", employerName },
            {"FindAnApprenticeshipAdvertURL", findAnApprenticeshipAdvertUrl},
            {"notificationSettingsURL", notificationSettingsUrl},
            {"VACcode", vacancyReference},
            {"location", location}
        };
        
        var actual = new VacancyReviewResponseEmailTemplate(templateId, recipientEmail, advertTitle, firstName, employerName,findAnApprenticeshipAdvertUrl, notificationSettingsUrl, vacancyReference, location);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}