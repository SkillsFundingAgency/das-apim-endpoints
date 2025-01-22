using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DeleteTrainingCourse;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.TrainingCoursesController;

public class WhenGettingDeleteTrainingCourse
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid applicationId,
           Guid trainingCourseId,
           GetDeleteTrainingCourseQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetDeleteTrainingCourseQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.TrainingCourseId == trainingCourseId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetDeleteTrainingCourse(applicationId, trainingCourseId, candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetTrainingCourseApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetTrainingCourseApiResponse)queryResult.Course);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Mediator_Returns_Null_Response_So_NotFound_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        Guid trainingCourseId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetDeleteTrainingCourseQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.TrainingCourseId == trainingCourseId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.GetDeleteTrainingCourse(applicationId, trainingCourseId, candidateId);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
        Guid candidateId,
        Guid applicationId,
        Guid trainingCourseId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetDeleteTrainingCourseQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.TrainingCourseId == trainingCourseId),
                It.IsAny<CancellationToken>()))
             .ThrowsAsync(new InvalidOperationException());

        var actual = await controller.GetDeleteTrainingCourse(applicationId, trainingCourseId, candidateId);

        actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
