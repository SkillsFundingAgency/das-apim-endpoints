using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessors.Api.Controllers;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.Application.Queries.GetTrainingCourses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.Api.UnitTests.Controllers
{
    public class WhenGettingTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_From_Mediator(
            GetTrainingCoursesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetTrainingCoursesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCourseListResponse;
            Assert.IsNotNull(model);
            model.Courses.Should().BeEquivalentTo(mediatorResult.TrainingCourses.Select(item => (GetCourseListItem)item));
        }
    }
}