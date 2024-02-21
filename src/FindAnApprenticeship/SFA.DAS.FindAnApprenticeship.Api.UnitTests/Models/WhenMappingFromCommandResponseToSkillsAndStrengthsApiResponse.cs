using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResponseToSkillsAndStrengthsApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetSkillsAndStrengthsQueryResult source)
    {
        var actual = (GetSkillsAndStrengthsApiResponse)source;

        using (new AssertionScope())
        {
            actual.ApplicationId.Should().Be(source.ApplicationId);
            actual.Employer.Should().Be(source.Employer);
            actual.ExpectedSkillsAndStrengths.Should().BeEquivalentTo(source.ExpectedSkillsAndStrengths);
        }
    }
}
