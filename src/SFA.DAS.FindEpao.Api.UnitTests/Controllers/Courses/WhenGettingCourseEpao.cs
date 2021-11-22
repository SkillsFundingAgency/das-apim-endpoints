using System.ComponentModel.DataAnnotations;
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
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.Api.UnitTests.Controllers.Courses
{
    public class WhenGettingCourseEpao
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Course_Epao_From_Mediator(
            int courseId,
            string epaoId,
            GetCourseEpaoResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetCourseEpaoQuery>(query => query.CourseId == courseId && query.EpaoId == epaoId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.CourseEpao(courseId, epaoId) as ObjectResult;


            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCourseEpaoResponse;
            model!.Course.Should().BeEquivalentTo((GetCourseListItem)mediatorResult.Course);
            model!.Epao.Should().BeEquivalentTo((EpaoDetails)mediatorResult.Epao);
            model!.CourseEpaosCount.Should().Be(mediatorResult.CourseEpaosCount);
            model!.EpaoDeliveryAreas.Should().BeEquivalentTo(
                    mediatorResult.EpaoDeliveryAreas.Select(area => (EpaoDeliveryArea) area));
            model!.DeliveryAreas.Should().BeEquivalentTo(
                mediatorResult.DeliveryAreas.Select(item => (GetDeliveryAreaListItem) item));
            model!.EffectiveFrom.Should().Be(mediatorResult.EffectiveFrom);
            model!.AllCourses.Should().BeEquivalentTo(
                    mediatorResult.AllCourses.Select(item => (GetCourseListItem)item));

        }

        [Test, MoqAutoData]
        public async Task And_ValidationException_Then_Returns_Bad_Request(
            int courseId,
            string epaoId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseEpaoQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<ValidationException>();

            var controllerResult = await controller.CourseEpao(courseId, epaoId) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test, MoqAutoData]
        public async Task And_NotFoundException_Then_Returns_Not_Found(
            int courseId,
            string epaoId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCourseEpaoQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<NotFoundException<GetCourseEpaoResult>>();

            var controllerResult = await controller.CourseEpao(courseId, epaoId) as NotFoundResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}