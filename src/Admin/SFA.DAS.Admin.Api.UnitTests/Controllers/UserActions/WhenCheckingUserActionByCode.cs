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
using SFA.DAS.Admin.Api.Controllers;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.Api.UnitTests.Controllers.UserActions
{
    public class WhenCheckingUserActionByCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserAction_Is_Returned(
            string code,
            CheckUserActionByCodeCommand command,
            CheckUserActionByCodeResult commandResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            command.Code = code;

            mediator
                .Setup(x => x.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync(commandResult);

            // Act
            var actual = await controller.CheckUserActionByCode(code, command) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(commandResult);

            mediator.Verify(m => m.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Command_From_Route_Is_Used_When_Body_Is_Null(
            string code,
            CheckUserActionByCodeResult commandResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            CheckUserActionByCodeCommand passedCommand = null;

            mediator
                .Setup(x => x.Send(It.IsAny<CheckUserActionByCodeCommand>(), CancellationToken.None))
                .Callback<IRequest<CheckUserActionByCodeResult>, CancellationToken>((c, ct) => { passedCommand = c as CheckUserActionByCodeCommand; })
                .ReturnsAsync(commandResult);

            // Act
            var actual = await controller.CheckUserActionByCode(code, null) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            passedCommand.Should().NotBeNull();
            passedCommand.Code.Should().Be(code);

            mediator.Verify(m => m.Send(It.IsAny<CheckUserActionByCodeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_If_Command_Result_Does_Not_Exist(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), CancellationToken.None))
                .ReturnsAsync((CheckUserActionByCodeResult)null);

            // Act
            var actual = await controller.CheckUserActionByCode(code, new CheckUserActionByCodeCommand { Code = code }) as NotFoundResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            mediator.Verify(m => m.Send(It.Is<CheckUserActionByCodeCommand>(q => q.Code == code), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string code,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<CheckUserActionByCodeCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CheckUserActionByCode(code, new CheckUserActionByCodeCommand { Code = code }) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CheckUserActionByCodeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
