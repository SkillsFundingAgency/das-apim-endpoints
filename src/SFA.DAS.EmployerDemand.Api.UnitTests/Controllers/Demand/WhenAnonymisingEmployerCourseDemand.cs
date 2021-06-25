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
using SFA.DAS.EmployerDemand.Application.Demand.Commands.AnonymiseDemand;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenAnonymisingEmployerCourseDemand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator(
            Guid employerDemandId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<AnonymiseDemandCommand>(command => command.EmployerDemandId == employerDemandId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);
            
            var controllerResult = await controller.AnonymiseEmployerDemand(employerDemandId) as OkResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
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
                    It.IsAny<AnonymiseDemandCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new HttpRequestContentException("Error", HttpStatusCode.BadRequest,errorContent));
            
            var controllerResult = await controller.AnonymiseEmployerDemand(employerDemandId) as ObjectResult;

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
                    It.IsAny<AnonymiseDemandCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.AnonymiseEmployerDemand(employerDemandId) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}