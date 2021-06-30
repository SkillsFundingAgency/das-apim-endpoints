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
using SFA.DAS.EmployerDemand.Api.Controllers;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenStoppingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator_And_Model_Returned(
            Guid employerDemandId,
            StopEmployerDemandCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<StopEmployerDemandCommand>(command => 
                        command.EmployerDemandId == employerDemandId
                        && command.Id != Guid.Empty
                    ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);
            
            var controllerResult = await controller.StopEmployerDemand(employerDemandId) as OkObjectResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            controllerResult.Value.Should().BeEquivalentTo((GetCourseDemandResponse)mediatorResult.EmployerDemand);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_HttpException_It_Is_Returned(
            string errorContent,
            Guid employerDemandId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<StopEmployerDemandCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.StopEmployerDemand(employerDemandId) as ObjectResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
            controllerResult.Value.Should().Be(errorContent);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_Bad_Request_Is_Returned(
            Guid employerDemandId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<StopEmployerDemandCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.StopEmployerDemand(employerDemandId) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}