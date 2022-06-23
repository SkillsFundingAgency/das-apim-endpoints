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
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.Api.UnitTests.Controllers.Courses
{
    public class WhenGettingCourseList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Training_Courses_From_Mediator(
            GetCourseListResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseListQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetList() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCourseListResponse;
            Assert.IsNotNull(model);
            model.Courses.Should().BeEquivalentTo(mediatorResult.Courses.Select(course => (GetCourseListItem)course));
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetList() as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}