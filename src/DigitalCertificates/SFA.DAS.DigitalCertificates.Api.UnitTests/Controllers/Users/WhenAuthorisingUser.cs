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
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenAuthorisingUser
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_NoContent_And_Mediator_Send_Called(
            Guid userId,
            CreateUserAuthoriseCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateUserAuthoriseCommand>(c =>
                    c.UserId == userId && c.Uln == command.Uln), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            // Act
            var actual = await controller.CreateUserAuthorise(userId, command) as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            mediator.Verify(m => m.Send(It.Is<CreateUserAuthoriseCommand>(c => c.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            CreateUserAuthoriseCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateUserAuthoriseCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateUserAuthorise(userId, command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateUserAuthoriseCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
