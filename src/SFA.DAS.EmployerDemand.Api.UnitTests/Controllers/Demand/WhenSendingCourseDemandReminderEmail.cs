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
using SFA.DAS.EmployerDemand.Application.Demand.Commands.SendEmployerDemandReminder;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Controllers.Demand
{
    public class WhenSendingCourseDemandReminderEmail
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator_And_Id_Returned(
            Guid id,
            Guid courseDemandId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] DemandController controller)
        {
            //Act
            var controllerResult = await controller.SendReminderEmail(courseDemandId, id) as CreatedResult;

            //Assert
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Value.Should().BeEquivalentTo(new {id});
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<SendEmployerDemandEmailReminderCommand>(command => 
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
                    It.IsAny<SendEmployerDemandEmailReminderCommand>(),
                    It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            var controllerResult = await controller.SendReminderEmail(id, courseDemandId) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}