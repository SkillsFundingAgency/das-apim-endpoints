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
using SFA.DAS.Admin.Application.Commands.UnlockUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.Api.UnitTests.Controllers.Users
{
    public class WhenUnlockingUser
    {
        [Test, MoqAutoData]
        public async Task Then_NoContent_Returned_When_Mediator_Returns_Unit(
            Guid userId,
            UnlockUserCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            UnlockUserCommand passed = null;

            mediator
                .Setup(x => x.Send(It.IsAny<UnlockUserCommand>(), CancellationToken.None))
                .Callback<IRequest<Unit?>, CancellationToken>((c, ct) => { passed = c as UnlockUserCommand; })
                .ReturnsAsync((Unit?)Unit.Value);

            // Act
            var result = await controller.UnlockUser(userId, command) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            passed.Should().NotBeNull();
            passed.UserId.Should().Be(userId);

            mediator.Verify(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Command_From_Route_Is_Used_When_Body_Is_Null(
            Guid userId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            UnlockUserCommand passed = null;

            mediator
                .Setup(x => x.Send(It.IsAny<UnlockUserCommand>(), CancellationToken.None))
                .Callback<IRequest<Unit?>, CancellationToken>((c, ct) => { passed = c as UnlockUserCommand; })
                .ReturnsAsync((Unit?)Unit.Value);

            // Act
            var result = await controller.UnlockUser(userId, null) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            passed.Should().NotBeNull();
            passed.UserId.Should().Be(userId);

            mediator.Verify(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_BadRequest_Returned_When_Mediator_Returns_Null(
            Guid userId,
            UnlockUserCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<UnlockUserCommand>(), CancellationToken.None))
                .ReturnsAsync((Unit?)null);

            // Act
            var result = await controller.UnlockUser(userId, command) as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            mediator.Verify(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            UnlockUserCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<UnlockUserCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.UnlockUser(userId, command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<UnlockUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
