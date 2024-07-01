using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates;

public class WhenBuildingSendSubmitApplicationEmailReminderTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string firstName, string vacancy, string vacancyUrl, int daysRemaining, string employer, string city, string postcode, string continueApplicationUrl, DateTime closingDate, string settingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"daysRemaining", daysRemaining.ToString() },
            {"vacancy", vacancy },
            {"vacancyUrl", vacancyUrl },
            {"employer", employer },
            {"city", city },
            {"postcode", postcode },
            {"closingDate", closingDate.ToString("d MMM yyyy") },
            {"continueApplicationLink", continueApplicationUrl },
            {"settingsUrl", settingsUrl }
        };

        var actual = new SendSubmitApplicationEmailReminderTemplate(templateId, recipientEmail, firstName,daysRemaining, vacancy, vacancyUrl, employer,city,postcode, continueApplicationUrl, closingDate, settingsUrl);

        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}