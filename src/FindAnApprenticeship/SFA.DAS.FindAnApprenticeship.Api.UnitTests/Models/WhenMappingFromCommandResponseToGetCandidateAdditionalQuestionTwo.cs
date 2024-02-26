using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateAdditionalQuestionTwo;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResponseToGetCandidateAdditionalQuestionTwo
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetCandidateAdditionalQuestionTwoQueryResult source)
    {
        var actual = (GetCandidateAdditionalQuestionTwoApiResponse)source;

        actual.QuestionTwo.Should().BeEquivalentTo(source.QuestionTwo);
    }
}
