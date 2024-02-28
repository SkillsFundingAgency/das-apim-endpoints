using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResponseToGetAdditionalQuestionApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetAdditionalQuestionQueryResult source)
    {
        var actual = (GetAdditionalQuestionApiResponse)source;

        actual.Question.Should().BeEquivalentTo(source.QuestionText);
    }
}
