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
using SFA.DAS.EmployerDemand.Application.Demand.Commands.SendAutomaticEmployerDemandDemandCutOff;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenSendingAutomaticTwelveWeekCutOffEmail
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator_And_Id_Returned(
            Guid id,
            Guid courseDemandId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            //Act
            var controllerResult = await controller.SendAutomaticCutOffEmail(courseDemandId, id) as CreatedResult;

            //Assert
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Value.Should().BeEquivalentTo(new {id});
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<SendAutomaticEmployerDemandDemandCutOffCommand>(command => 
                        command.Id == id
                        && command.EmployerDemandId == courseDemandId
                    ),
                    It.IsAny<CancellationToken>()));
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_Bad_Request_Is_Returned(
            Guid id,
            Guid courseDemandId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SendAutomaticEmployerDemandDemandCutOffCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.SendAutomaticCutOffEmail(id, courseDemandId) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}