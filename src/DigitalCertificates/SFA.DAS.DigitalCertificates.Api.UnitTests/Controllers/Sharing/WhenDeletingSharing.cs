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
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenDeletingSharing
    {
        [Test, MoqAutoData]
        public async Task Then_NoContent_Is_Returned_On_Success(
            Guid sharingId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), CancellationToken.None))
                .Returns(Task.CompletedTask);

            // Act
            var actual = await controller.DeleteSharing(sharingId) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            mediator.Verify(m => m.Send(It.Is<DeleteSharingCommand>(c => c.SharingId == sharingId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid sharingId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<DeleteSharingCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.DeleteSharing(sharingId) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<DeleteSharingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
