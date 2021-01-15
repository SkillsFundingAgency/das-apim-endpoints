using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Controllers;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Controllers.Epaos
{
    public class WhenGettingEpaoCourses
    {
        [Test, MoqAutoData]
        public async Task And_ValidationException_Then_Returns_BadRequest(
            string epaoId,
            ValidationException exception,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EpaosController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEpaoCoursesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var controllerResult = await controller.GetEpaoCourses(epaoId) as BadRequestObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            controllerResult!.Value.Should().Be(exception.Message);
        }

        [Test, MoqAutoData]
        public async Task And_EntityNotFoundException_Then_Returns_NotFound(
            string epaoId,
            NotFoundException<GetEpaoCoursesResult> notFoundException,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EpaosController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEpaoCoursesQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(notFoundException);

            var controllerResult = await controller.GetEpaoCourses(epaoId) as NotFoundResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_EpaoCourses_From_Mediator(
            string epaoId,
            GetEpaoCoursesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EpaosController controller)
        {
            controller.Url = Mock.Of<IUrlHelper>();
            var expectedModel = (GetEpaoCoursesApiModel) mediatorResult;
            expectedModel.BuildLinks(controller.Url);
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetEpaoCoursesQuery>(query => query.EpaoId == epaoId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetEpaoCourses(epaoId) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetEpaoCoursesApiModel;
            model.Should().BeEquivalentTo(expectedModel);
        }
    }
}
