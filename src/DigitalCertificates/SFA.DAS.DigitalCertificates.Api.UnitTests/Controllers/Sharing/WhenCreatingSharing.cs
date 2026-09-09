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
using SFA.DAS.DigitalCertificates.Api.Models.Sharing;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenCreatingSharing
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Details_Are_Returned(
            CreateSharingRequest request,
            CreateSharingResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateSharingCommand>(c =>
                    c.UserId == request.UserId &&
                    c.CertificateId == request.CertificateId &&
                    c.CertificateType == request.CertificateType &&
                    c.CourseName == request.CourseName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actual = await controller.CreateSharing(request) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            mediator.Verify(m => m.Send(It.IsAny<CreateSharingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            CreateSharingRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateSharingCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateSharing(request) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateSharingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}