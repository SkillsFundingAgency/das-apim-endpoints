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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Certificates
{
    public class WhenCreatingPrintRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_NoContent_And_Mediator_Send_Called(
            Guid id,
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CertificatesController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateCertificatePrintRequestCommand>(c => c.CertificateId == id), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            // Act
            var actual = await controller.CreatePrintRequest(id, command) as NoContentResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            mediator.Verify(m => m.Send(It.Is<CreateCertificatePrintRequestCommand>(c => c.CertificateId == id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid id,
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CertificatesController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateCertificatePrintRequestCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreatePrintRequest(id, command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateCertificatePrintRequestCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_BadRequest_Returned_If_ArgumentException_Thrown(
            Guid id,
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] CertificatesController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateCertificatePrintRequestCommand>(), CancellationToken.None))
                .ThrowsAsync(new ArgumentException());

            // Act
            var actual = await controller.CreatePrintRequest(id, command) as BadRequestResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            mediator.Verify(m => m.Send(It.IsAny<CreateCertificatePrintRequestCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
