using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmployerAdditionalQuestionTwo;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResponseToGetEmployerAdditionalQuestionTwoApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetEmployerAdditionalQuestionTwoQueryResult source)
    {
        var actual = (GetAdditionalQuestionTwoApiResponse)source;

        actual.QuestionTwo.Should().BeEquivalentTo(source.QuestionTwo);
    }
}
