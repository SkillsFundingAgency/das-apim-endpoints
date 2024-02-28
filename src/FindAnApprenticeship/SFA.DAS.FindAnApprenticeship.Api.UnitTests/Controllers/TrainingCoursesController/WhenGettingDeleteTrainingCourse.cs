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
}
