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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenGettingSharedStandardCertificate
    {
        [Test, MoqAutoData]
        public async Task Then_The_Shared_Certificate_Is_Returned(
            Guid id,
            GetSharedStandardCertificateQueryResult expectedResponse,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.Is<GetSharedStandardCertificateQuery>(q => q.Id == id), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var actual = await controller.GetSharedStandardCertificate(id) as OkObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(expectedResponse);

            mediator.Verify(m => m.Send(It.Is<GetSharedStandardCertificateQuery>(q => q.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_When_Certificate_Not_Found(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            GetSharedStandardCertificateQueryResult? expectedResponse = null;
            mediator
                .Setup(m => m.Send(It.IsAny<GetSharedStandardCertificateQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await controller.GetSharedStandardCertificate(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();

            mediator.Verify(m => m.Send(It.Is<GetSharedStandardCertificateQuery>(q => q.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_Exception_Is_Thrown(
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] SharingController controller)
        {
            // Arrange
            mediator
                .Setup(m => m.Send(It.IsAny<GetSharedStandardCertificateQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetSharedStandardCertificate(id) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetSharedStandardCertificateQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
