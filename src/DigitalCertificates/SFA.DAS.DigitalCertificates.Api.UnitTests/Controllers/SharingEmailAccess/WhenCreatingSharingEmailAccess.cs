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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.SharingEmailAccess
{
    public class WhenCreatingSharingEmailAccess
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_And_Mediator_Send_Called(
            CreateSharingEmailAccessCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingEmailAccessController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateSharingEmailAccessCommand>(c => c.SharingEmailId == command.SharingEmailId), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            // Act
            var actual = await controller.CreateSharingEmailAccess(command) as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            mediator.Verify(m => m.Send(It.Is<CreateSharingEmailAccessCommand>(c => c.SharingEmailId == command.SharingEmailId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            CreateSharingEmailAccessCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingEmailAccessController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateSharingEmailAccessCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateSharingEmailAccess(command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateSharingEmailAccessCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
   }
}
