using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain.EmailTemplates;

namespace SFA.DAS.Recruit.UnitTests.Domain.EmailTemplates;

public class WhenBuildingApplicationResponseSuccessEmailTemplate
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"location", $"{city}, {postcode}" }
        };
        
        var actual = new ApplicationResponseSuccessEmailTemplate(templateId, recipientEmail, firstName, vacancy, employer, city, postcode);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
    
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly_With_No_Postcode(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"location", city }
        };
        
        var actual = new ApplicationResponseSuccessEmailTemplate(templateId, recipientEmail, firstName, vacancy, employer, city, null!);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
    
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly_With_No_City(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string city, string postcode)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"location", postcode }
        };
        
        var actual = new ApplicationResponseSuccessEmailTemplate(templateId, recipientEmail, firstName, vacancy, employer, null!, postcode);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }

}