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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenCreatingSharing
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Details_Are_Returned(
            CreateSharingCommand command,
            CreateSharingResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateSharingCommand>(c =>
                    c.UserId == command.UserId &&
                    c.CertificateId == command.CertificateId &&
                    c.CertificateType == command.CertificateType &&
                    c.CourseName == command.CourseName), CancellationToken.None))
                .ReturnsAsync(result);

            // Act
            var actual = await controller.CreateSharing(command) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(result);

            mediator.Verify(m => m.Send(It.Is<CreateSharingCommand>(c =>
                c.UserId == command.UserId &&
                c.CertificateId == command.CertificateId &&
                c.CertificateType == command.CertificateType &&
                c.CourseName == command.CourseName), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            CreateSharingCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateSharingCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateSharing(command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateSharingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}