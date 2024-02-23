using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
using System.Threading;
using FluentAssertions;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.TrainingCoursesController;
public class WhenGettingTrainingCourse
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid applicationId,
           Guid trainingCourseId,
           GetTrainingCourseQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCourseQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.TrainingCourseId == trainingCourseId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetTrainingCourse(applicationId, trainingCourseId, candidateId);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetTrainingCourseApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetTrainingCourseApiResponse)queryResult.Course);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Mediator_Response_Is_Null_Then_Returns_NotFound(
           Guid candidateId,
           Guid applicationId,
           Guid trainingCourseId,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetTrainingCourseQuery>(q =>
                    q.CandidateId == candidateId
                    && q.ApplicationId == applicationId
                    && q.TrainingCourseId == trainingCourseId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.GetTrainingCourse(applicationId, trainingCourseId, candidateId);

        actual.Should().BeOfType<NotFoundResult>();
    }
}
