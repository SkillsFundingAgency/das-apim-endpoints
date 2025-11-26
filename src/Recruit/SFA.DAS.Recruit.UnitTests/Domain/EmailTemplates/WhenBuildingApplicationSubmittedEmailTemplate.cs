using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Domain.EmailTemplates;

namespace SFA.DAS.Recruit.UnitTests.Domain.EmailTemplates;

public class WhenBuildingApplicationSubmittedEmailTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string advertTitle, string firstName, string vacancyReference, string employerName, string location, string manageAdvertUrl, string notificationSettingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"advertTitle", advertTitle },
            {"firstName", firstName },
            {"employerName", employerName },
            {"manageAdvertURL", manageAdvertUrl },
            {"notificationSettingsURL", notificationSettingsUrl },
            {"VACcode", vacancyReference},
            {"location", location}
        };
        
        var actual = new ApplicationSubmittedEmailTemplate(templateId, recipientEmail, advertTitle, firstName, vacancyReference, employerName, location, manageAdvertUrl, notificationSettingsUrl);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}
