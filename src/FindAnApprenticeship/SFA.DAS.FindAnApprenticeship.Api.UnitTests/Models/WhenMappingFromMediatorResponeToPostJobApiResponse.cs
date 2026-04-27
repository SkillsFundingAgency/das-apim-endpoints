using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromMediatorResponeToPostJobApiResponse
{
    [Test, MoqAutoData]
    public void Then_The_Response_Is_Mapped(CreateJobCommandResult source)
    {
        var actual = (PostJobApiResponse)source;

        actual.Id.Should().Be(source.Id);
    }
}
