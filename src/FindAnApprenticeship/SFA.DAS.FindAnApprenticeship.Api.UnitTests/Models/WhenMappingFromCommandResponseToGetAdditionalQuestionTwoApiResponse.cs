using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestionTwo;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResponseToGetAdditionalQuestionTwoApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetAdditionalQuestionTwoQueryResult source)
    {
        var actual = (GetAdditionalQuestionTwoApiResponse)source;

        actual.QuestionTwo.Should().BeEquivalentTo(source.QuestionTwo);
    }
}
