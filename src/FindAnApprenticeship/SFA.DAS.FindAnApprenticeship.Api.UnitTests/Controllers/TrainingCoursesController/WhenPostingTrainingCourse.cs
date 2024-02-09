﻿using System;
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
        PostTrainingCourseApiRequest apiRequest,
        CreateTrainingCourseCommandResponse response,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.Is<CreateTrainingCourseCommand>(c =>
            c.CandidateId.Equals(apiRequest.CandidateId)
            && c.ApplicationId == applicationId
            && c.TrainingProviderName == apiRequest.TrainingProviderName
            && c.CourseName == apiRequest.CourseName
            && c.YearAchieved == apiRequest.YearAchieved),
            CancellationToken.None))
            .ReturnsAsync(response);

        var actual = await controller.PostTrainingCourse(applicationId, apiRequest) as CreatedResult;
        var actualModel = actual?.Value as PostTrainingCourseApiResponse;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actualModel.Should().NotBeNull();
            actualModel.Should().BeEquivalentTo((PostTrainingCourseApiResponse)response);
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

    [Test, MoqAutoData]
    public async Task And_Null_Is_Returned_Then_Not_Found_Response_Returned(
        Guid applicationId,
        PostTrainingCourseApiRequest apiRequest,
        [Frozen]
        Mock<IMediator> mediator,
        [Greedy] Api.Controllers.TrainingCoursesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<CreateTrainingCourseCommand>(), CancellationToken.None)).ReturnsAsync(() => null);

        var actual = await controller.PostTrainingCourse(applicationId, apiRequest) as StatusCodeResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}