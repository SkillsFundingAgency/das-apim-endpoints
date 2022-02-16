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
using SFA.DAS.ApimDeveloper.Api.ApiRequests;
using SFA.DAS.ApimDeveloper.Api.Controllers;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.SendEmailToChangePassword;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.Controllers
{
    public class WhenSendingChangePasswordEmail
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Is_Processed_By_Mediator_And_Id_Returned(
            Guid id,
            SendChangePasswordEmailRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            //Act
            var controllerResult = await controller.SendChangePasswordEmail(id, request) as CreatedResult;

            //Assert
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Value.Should().BeEquivalentTo(new {id});
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<SendEmailToChangePasswordCommand>(command => 
                        command.FirstName == request.FirstName
                        && command.LastName == request.LastName
                        && command.Email == request.Email
                        && command.ChangePasswordUrl == request.ChangePasswordUrl
                    ),
                    It.IsAny<CancellationToken>()));
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_There_Is_An_Error_A_Bad_Request_Is_Returned(
            Guid id,
            SendChangePasswordEmailRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<SendEmailToChangePasswordCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            
            var controllerResult = await controller.SendChangePasswordEmail(id, request) as StatusCodeResult;

            controllerResult!.StatusCode.Should().Be((int) HttpStatusCode.InternalServerError);
        }
    }
}