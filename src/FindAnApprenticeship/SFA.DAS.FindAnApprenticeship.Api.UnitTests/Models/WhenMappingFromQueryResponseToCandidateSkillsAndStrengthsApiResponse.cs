using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromQueryResponseToCandidateSkillsAndStrengthsApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetCandidateSkillsAndStrengthsQueryResult source)
    {
        var actual = (GetCandidateSkillsAndStrengthsApiResponse)source;
        actual.Strengths.Should().BeEquivalentTo(source.Strengths);
    }
}
