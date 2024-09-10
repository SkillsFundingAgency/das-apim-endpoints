using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Models;

public class WhenBuildingWithdrawApplicationEmail
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"city", city },
            {"postcode", postcode }
        };
        
        var actual = new WithdrawApplicationEmail(templateId, recipientEmail, firstName, vacancy, employer, city, postcode);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}