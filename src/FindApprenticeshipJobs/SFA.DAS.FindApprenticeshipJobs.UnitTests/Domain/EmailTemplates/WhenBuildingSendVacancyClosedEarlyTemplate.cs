using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Domain.EmailTemplates;

public class WhenBuildingSendVacancyClosedEarlyTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string firstName, string vacancy, string vacancyUrl, string employer, string city, string postcode, DateTime dateApplicationStarted, string settingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"vacancyUrl", vacancyUrl },
            {"employer", employer },
            {"location", $"{city}, {postcode}" },
            {"dateApplicationStarted", dateApplicationStarted.ToString("d MMM yyyy") },
            {"settingsUrl", settingsUrl }
        };

        var actual = new SendVacancyClosedEarlyTemplate(templateId, recipientEmail, firstName, vacancy, vacancyUrl, employer,city,postcode, dateApplicationStarted, settingsUrl);

        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
    
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly_With_No_City(string templateId,string recipientEmail, string firstName, string vacancy, string vacancyUrl, string employer, string postcode, DateTime dateApplicationStarted, string settingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"vacancyUrl", vacancyUrl },
            {"employer", employer },
            {"location", $"{postcode}" },
            {"dateApplicationStarted", dateApplicationStarted.ToString("d MMM yyyy") },
            {"settingsUrl", settingsUrl }
        };

        var actual = new SendVacancyClosedEarlyTemplate(templateId, recipientEmail, firstName, vacancy, vacancyUrl, employer,null!, postcode, dateApplicationStarted, settingsUrl);

        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
    
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly_With_No_Postcode(string templateId,string recipientEmail, string firstName, string vacancy, string vacancyUrl, string employer, string city, DateTime dateApplicationStarted, string settingsUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"vacancyUrl", vacancyUrl },
            {"employer", employer },
            {"location", $"{city}" },
            {"dateApplicationStarted", dateApplicationStarted.ToString("d MMM yyyy") },
            {"settingsUrl", settingsUrl }
        };

        var actual = new SendVacancyClosedEarlyTemplate(templateId, recipientEmail, firstName, vacancy, vacancyUrl, employer,city, null!, dateApplicationStarted, settingsUrl);

        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}