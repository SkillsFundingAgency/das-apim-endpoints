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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenCreatingSharingAccess
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_And_Mediator_Send_Called(
            CreateSharingAccessCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateSharingAccessCommand>(c => c.SharingId == command.SharingId), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            // Act
            var actual = await controller.CreateSharingAccess(command) as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            mediator.Verify(m => m.Send(It.Is<CreateSharingAccessCommand>(c => c.SharingId == command.SharingId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            CreateSharingAccessCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateSharingAccessCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateSharingAccess(command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateSharingAccessCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
