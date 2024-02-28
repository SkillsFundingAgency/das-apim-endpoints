using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Api.Controllers;
using SFA.DAS.FindEpao.Api.Models;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourse;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.Api.UnitTests.Controllers.Courses
{
    public class WhenGettingACourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_From_Mediator(
            int courseId,
            GetCourseResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseQuery>(c => c.CourseId.Equals(courseId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(courseId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCourseResponse;
            Assert.That(model, Is.Not.Null);
            model.Course.Should().BeEquivalentTo(mediatorResult.Course, options => options
            .Excluding(c => c.LarsCode)
            .Excluding(c => c.StandardUId)
            .Excluding(c => c.StandardVersions));

            model.Course.Id.Should().Be(mediatorResult.Course.LarsCode);

        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int courseId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(courseId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}