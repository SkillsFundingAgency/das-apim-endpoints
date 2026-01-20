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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenCreatingSharingEmail
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Email_Result_Is_Returned(
            Guid sharingId,
            CreateSharingEmailCommand command,
            CreateSharingEmailResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateSharingEmailCommand>(c =>
                    c.SharingId == sharingId &&
                    c.EmailAddress == command.EmailAddress &&
                    c.TemplateId == command.TemplateId), CancellationToken.None))
                .ReturnsAsync(result);

            // Act
            var actual = await controller.CreateSharingEmail(sharingId, command) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(result);

            mediator.Verify(m => m.Send(It.Is<CreateSharingEmailCommand>(c =>
                c.SharingId == sharingId &&
                c.EmailAddress == command.EmailAddress &&
                c.TemplateId == command.TemplateId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid sharingId,
            CreateSharingEmailCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateSharingEmailCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateSharingEmail(sharingId, command) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateSharingEmailCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
