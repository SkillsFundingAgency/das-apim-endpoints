using System;
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
using SFA.DAS.FindEpao.Api.Controllers;
using SFA.DAS.FindEpao.Api.Models;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.Api.UnitTests.Controllers.Courses
{
    public class WhenGettingCourseEpaos
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Course_Epaos_From_Mediator(
            int courseId,
            GetCourseEpaosResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseEpaosQuery>(query => query.CourseId == courseId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.CourseEpaos(courseId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCourseEpaoListResponse;
            Assert.That(model, Is.Not.Null);
            model.Course.Should().BeEquivalentTo((GetCourseListItem)mediatorResult.Course);
            model.Epaos.Should().BeEquivalentTo(mediatorResult.Epaos.Select(item => (GetCourseEpaoListItem)item));
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int courseId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseEpaosQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.CourseEpaos(courseId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}