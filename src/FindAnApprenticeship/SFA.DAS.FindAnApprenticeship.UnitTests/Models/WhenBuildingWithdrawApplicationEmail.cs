using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Models;

public class WhenBuildingWithdrawApplicationEmail
{
    [Test, AutoData]
    public void Then_The_Values_Are_Set_Correctly(string templateId,string recipientEmail, string firstName, string vacancy, string employer, string location)
    {
        var expectedTokens = new Dictionary<string, string>
        {
            {"firstName", firstName },
            {"vacancy", vacancy },
            {"employer", employer },
            {"location", location }
        };
        
        var actual = new WithdrawApplicationEmail(templateId, recipientEmail, firstName, vacancy, employer, location);
        
        actual.TemplateId.Should().Be(templateId);
        actual.Tokens.Should().BeEquivalentTo(expectedTokens);
        actual.RecipientAddress.Should().Be(recipientEmail);
    }
}