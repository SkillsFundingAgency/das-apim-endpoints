using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateSkillsAndStrengthsCommand;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromCommandResultToPostSkillsAndStrengthsApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(CreateSkillsAndStrengthsCommandResult source)
    {
        var actual = (PostSkillsAndStrengthsApiResponse)source;

        actual.Id.Should().Be(source.Id);
    }
}
