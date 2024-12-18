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

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("b648c047-b2e3-4ebe-b9b4-a6cc59c2af94");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("8387c857-b4f9-4cfb-8c44-df4c2560e446");
    }
    
    [Test]
    public void Then_The_TemplateIds_Are_Correctly_Set_For_Non_Production()
    {
        var actual = new EmailEnvironmentHelper("teST");

        actual.SuccessfulApplicationEmailTemplateId.Should().Be("35e8b348-5f26-488a-8165-459522f8189b");
        actual.UnsuccessfulApplicationEmailTemplateId.Should().Be("95d7ff0c-79fc-4585-9fff-5e583b478d23");
    }
}