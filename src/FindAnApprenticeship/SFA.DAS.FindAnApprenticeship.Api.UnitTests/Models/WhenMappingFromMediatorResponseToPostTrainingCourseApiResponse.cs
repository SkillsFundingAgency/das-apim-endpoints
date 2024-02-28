using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromMediatorResponseToPostTrainingCourseApiResponse
{
    [Test, MoqAutoData]
    public void Then_The_Response_Is_Mapped(CreateTrainingCourseCommandResult source)
    {
        var actual = (PostTrainingCourseApiResponse)source;

        actual.Id.Should().Be(source.Id);
    }
}
