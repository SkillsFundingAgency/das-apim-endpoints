using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromQueryResponseToCandidateSkillsAndStrengthsApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetCandidateSkillsAndStrengthsQueryResult source)
    {
        var actual = (GetCandidateSkillsAndStrengthsApiResponse)source;

        using (new AssertionScope())
        {
            actual.AboutYou.ApplicationId.Should().Be((Guid)source.AboutYou.ApplicationId);
            actual.AboutYou.Improvements.Should().BeEquivalentTo(source.AboutYou.Improvements);
            actual.AboutYou.HobbiesAndInterests.Should().BeEquivalentTo(source.AboutYou.HobbiesAndInterests);
            actual.AboutYou.Support.Should().BeEquivalentTo(source.AboutYou.Support);
        }
    }
}
