using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Models;

public class WhenCreatingEmailEnvironmentHelper
{
    [Test]
    public void Then_When_Prod_Environment_Template_And_ApplicationUrl_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("PRd");

        actual.SubmitApplicationEmailTemplateId.Should().Be("ce8a2834-83ad-4b84-83a1-099f593cd313");
        actual.WithdrawApplicationEmailTemplateId.Should().Be("d81b30cd-df12-46ed-9459-7e914f1f8a3b");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
    }
    [Test]
    public void Then_When_Non_Prod_Environment_Template_And_ApplicationUrl_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("TEST");

        actual.SubmitApplicationEmailTemplateId.Should().Be("4b584d3c-7f56-4fd1-95fd-3099ddcb2ffa");
        actual.WithdrawApplicationEmailTemplateId.Should().Be("983607b3-742f-4bec-b2cc-7846cbe4368d");
        actual.CandidateApplicationUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/applications");
    }
}