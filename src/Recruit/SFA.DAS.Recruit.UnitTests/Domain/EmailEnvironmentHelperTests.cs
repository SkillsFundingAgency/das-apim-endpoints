using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.UnitTests.Domain;

public class EmailEnvironmentHelperTests
{
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Production()
    {
        var actual = new EmailEnvironmentHelper("PRd");

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("7bc576f8-7bf5-4699-8a5b-6be8df7c1e50");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("0ad70535-c9d6-4b2e-ad17-d73e023d3387");
    }
    
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Non_Production()
    {
        var actual = new EmailEnvironmentHelper("teST");

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("5b3a4259-286a-41f7-b593-dfc9b9fcd6f9");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("9beb67ac-93c9-4704-ada9-6e417d29356b");
    }
}