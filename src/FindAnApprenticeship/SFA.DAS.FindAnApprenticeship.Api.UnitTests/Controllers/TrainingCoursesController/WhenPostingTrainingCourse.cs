using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateTrainingCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.TrainingCoursesController;
public class WhenPostingTrainingCourse
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Response_Is_Returned(
        Guid applicationId,
        CreateTrainingCourseCommandResult commandResult,
        PostTrainingCourseApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<CreateTrainingCourseCommand>(c =>
            c.CandidateId.Equals(apiRequest.CandidateId)
            && c.ApplicationId == applicationId
            && c.CourseName == apiRequest.CourseName
            && c.YearAchieved == (int)apiRequest.YearAchieved),
            CancellationToken.None))
            .ReturnsAsync(commandResult);

        var actual = await controller.PostTrainingCourse(applicationId, apiRequest);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<CreatedResult>();
            actual.As<CreatedResult>().Value.Should().BeEquivalentTo((PostTrainingCourseApiResponse)commandResult);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Command_Response_Is_Null_Then_NotFound_Returned(
        Guid applicationId,
        CreateTrainingCourseCommandResult commandResult,
        PostTrainingCourseApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<CreateTrainingCourseCommand>(c =>
            c.CandidateId.Equals(apiRequest.CandidateId)
            && c.ApplicationId == applicationId
            && c.CourseName == apiRequest.CourseName
            && c.YearAchieved == apiRequest.YearAchieved),
            CancellationToken.None))
            .ReturnsAsync(() => null);

        var actual = await controller.PostTrainingCourse(applicationId, apiRequest);

        using (new AssertionScope())
        {
            actual.Should().BeOfType<NotFoundResult>();
        }
    }

    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
        Guid applicationId,
        PostTrainingCourseApiRequest apiRequest,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<CreateTrainingCourseCommand>(), CancellationToken.None)).ThrowsAsync(new Exception());

        var actual = await controller.PostTrainingCourse(applicationId, apiRequest) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
