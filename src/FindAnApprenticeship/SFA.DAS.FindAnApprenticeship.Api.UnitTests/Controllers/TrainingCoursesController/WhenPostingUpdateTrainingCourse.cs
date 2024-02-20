using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateTrainingCourse;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.TrainingCoursesController;
public class WhenPostingUpdateTrainingCourse
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        Guid trainingCourseId,
        UpdateTrainingCourseCommandResult commandResult,
        PostUpdateTrainingCourseApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateTrainingCourseCommand>(c =>
                c.TrainingCourseId == trainingCourseId
                && c.CandidateId.Equals(apiRequest.CandidateId)
                && c.ApplicationId == applicationId
                && c.CourseName == apiRequest.CourseName
                && c.YearAchieved == apiRequest.YearAchieved),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        var actual = await controller.PostUpdateTrainingCourse(applicationId, trainingCourseId, apiRequest);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<OkObjectResult>();
            actual.As<OkObjectResult>().Value.Should().BeEquivalentTo(commandResult.Id);
        }
    }

    [Test, MoqAutoData]
    public async Task And_CommandResult_Is_Null_Then_Returns_NotFound(
        Guid applicationId,
        Guid trainingCourseId,
        PostUpdateTrainingCourseApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateTrainingCourseCommand>(c =>
                c.TrainingCourseId == trainingCourseId
                && c.CandidateId.Equals(apiRequest.CandidateId)
                && c.ApplicationId == applicationId
                && c.CourseName == apiRequest.CourseName
                && c.YearAchieved == apiRequest.YearAchieved),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var actual = await controller.PostUpdateTrainingCourse(applicationId, trainingCourseId, apiRequest);

        actual.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Thrown_Then_Internal_Server_Error_Returned(
        Guid applicationId,
        Guid trainingCourseId,
        PostUpdateTrainingCourseApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<UpdateTrainingCourseCommand>(c =>
                c.TrainingCourseId == trainingCourseId
                && c.CandidateId.Equals(apiRequest.CandidateId)
                && c.ApplicationId == applicationId
                && c.CourseName == apiRequest.CourseName
                && c.YearAchieved == apiRequest.YearAchieved),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var actual = await controller.PostUpdateTrainingCourse(applicationId, trainingCourseId, apiRequest) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
