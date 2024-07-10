using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResponseToExpectedSkillsAndStrengthsApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetExpectedSkillsAndStrengthsQueryResult source)
    {
        var actual = (GetExpectedSkillsAndStrengthsApiResponse)source;

        using (new AssertionScope())
        {
            actual.ApplicationId.Should().Be(source.ApplicationId);
            actual.Employer.Should().Be(source.Employer);
            actual.ExpectedSkillsAndStrengths.Should().BeEquivalentTo(source.ExpectedSkillsAndStrengths);
            actual.IsSectionCompleted.Should().Be(source.IsSectionCompleted);
        }
    }
}
