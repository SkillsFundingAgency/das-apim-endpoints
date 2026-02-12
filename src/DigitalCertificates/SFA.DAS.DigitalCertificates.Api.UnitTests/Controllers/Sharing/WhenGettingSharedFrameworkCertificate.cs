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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenGettingSharedFrameworkCertificate
    {
        [Test, MoqAutoData]
        public async Task Then_The_Shared_Framework_Certificate_Is_Returned(
            Guid id,
            GetSharedFrameworkCertificateQueryResult expectedResponse,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetSharedFrameworkCertificateQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var actual = await controller.GetSharedFrameworkCertificate(id) as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(expectedResponse);

            mediator.Verify(m => m.Send(It.Is<GetSharedFrameworkCertificateQuery>(q => q.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_When_Certificate_Not_Found(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            GetSharedFrameworkCertificateQueryResult? expectedResponse = null;
            mediator
                .Setup(m => m.Send(It.IsAny<GetSharedFrameworkCertificateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.GetSharedFrameworkCertificate(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            mediator.Verify(m => m.Send(It.Is<GetSharedFrameworkCertificateQuery>(q => q.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_Exception_Is_Thrown(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetSharedFrameworkCertificateQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetSharedFrameworkCertificate(id) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetSharedFrameworkCertificateQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
