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
using SFA.DAS.Assessors.Api.Controllers;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.Application.Queries.GetStandardDetails;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.Api.UnitTests.Controllers
{
    public class WhenGettingTrainingCourseById
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Training_Course_From_Mediator(
            string id,
            GetStandardDetailsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardDetailsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetStandardById(id) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetStandardDetailsResponse;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult.StandardDetails);
        }

        [Test, MoqAutoData]
        public async Task And_Invalid_StandardId_Then_Returns_Not_Found(
            string id,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] TrainingCoursesController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetStandardDetailsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetStandardDetailsResult(null));

            var controllerResult = await controller.GetStandardById(id) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}