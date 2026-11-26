using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.TrainingCoursesController;
public class WhenGettingTrainingCourses
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid applicationId,
           GetTrainingCoursesQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCoursesQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetTrainingCourses(applicationId, candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetTrainingCoursesApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetTrainingCoursesApiResponse)queryResult);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Mediator_Returns_Null_Then_Returns_NotFound(
           Guid candidateId,
           Guid applicationId,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCoursesQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.GetTrainingCourses(applicationId, candidateId) as StatusCodeResult;

        actual.StatusCode.Should().Be(404);
    }
}
