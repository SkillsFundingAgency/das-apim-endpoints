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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenCreatingCertificateSharing
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Details_Are_Returned(
            CreateCertificateSharingCommand command,
            CreateCertificateSharingResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            mediator
                .Setup(x => x.Send(It.Is<CreateCertificateSharingCommand>(c =>
                    c.UserId == command.UserId &&
                    c.CertificateId == command.CertificateId &&
                    c.CertificateType == command.CertificateType &&
                    c.CourseName == command.CourseName), CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.CreateCertificateSharing(command) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(result);

            mediator.Verify(m => m.Send(It.Is<CreateCertificateSharingCommand>(c =>
                c.UserId == command.UserId &&
                c.CertificateId == command.CertificateId &&
                c.CertificateType == command.CertificateType &&
                c.CourseName == command.CourseName), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            CreateCertificateSharingCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateCertificateSharingCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.CreateCertificateSharing(command) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateCertificateSharingCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}