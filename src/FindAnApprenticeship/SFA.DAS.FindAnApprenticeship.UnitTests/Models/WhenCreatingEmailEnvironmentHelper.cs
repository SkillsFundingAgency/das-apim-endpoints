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

        actual.SubmitApplicationEmailTemplateId.Should().Be("a07e4767-6cd6-44d1-8e65-044a83f434ad");
        actual.WithdrawApplicationEmailTemplateId.Should().Be("844b7dd8-c2cf-414c-ae10-45a6b42614b7");
        actual.CandidateApplicationUrl.Should().Be("https://findapprenticeship.service.gov.uk/applications");
    }
    [Test]
    public void Then_When_Non_Prod_Environment_Template_And_ApplicationUrl_Are_Set()
    {
        var actual = new EmailEnvironmentHelper("TEST");

        actual.SubmitApplicationEmailTemplateId.Should().Be("4b765435-ac6f-4d56-93ab-2f0f52402fb5");
        actual.WithdrawApplicationEmailTemplateId.Should().Be("e0c39593-4eed-46bf-9f3d-09c0cd3b046b");
        actual.CandidateApplicationUrl.Should().Be("https://test-findapprenticeship.apprenticeships.education.gov.uk/applications");
    }
}