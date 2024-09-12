using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain.EmailTemplates;

namespace SFA.DAS.Recruit.UnitTests.Domain.EmailTemplates;

public class WhenBuildingApplicationUnsuccessfulResponseEmailTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode, string feedback, string applicationUrl)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"city", city },
            {"postcode", postcode },
            {"feedback", feedback },
            {"applicationUrl", applicationUrl },
        };
        
        var actual = new ApplicationResponseUnsuccessfulEmailTemplate(templateId, recipientEmail, firstName, vacancy, employer, city, postcode, feedback, applicationUrl);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }

}