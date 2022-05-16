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
using SFA.DAS.FindAnApprenticeship.Api.ApiResponses;
using SFA.DAS.FindAnApprenticeship.Api.Controllers;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers
{
    public class WhenGettingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            GetCoursesQueryResult mediatorResult,
            [Frozen]Mock<IMediator> mediator,
            [Greedy] CoursesController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetCoursesQuery>(), CancellationToken.None))
                .ReturnsAsync(mediatorResult);
            
            var controllerResult = await controller.GetAll() as ObjectResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCoursesResponse;
            model.TrainingProgrammes.Should().BeEquivalentTo(mediatorResult.TrainingProgrammes);

        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Exception_Then_Internal_Server_Error_Returned(
            [Frozen]Mock<IMediator> mediator,
            [Greedy] CoursesController controller)
        {
            mediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCoursesQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetAll() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}