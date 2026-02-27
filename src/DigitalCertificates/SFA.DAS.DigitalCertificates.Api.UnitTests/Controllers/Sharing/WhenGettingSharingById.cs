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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Sharing
{
    public class WhenGettingSharingById
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Is_Returned(
        Guid sharingId,
        int limit,
        GetSharingByIdQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SharingController controller)
        {
            // Arrange
            mediator
            .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q =>
            q.SharingId == sharingId &&
            q.Limit == limit), CancellationToken.None))
            .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharingById(sharingId, limit) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult.Response);

            mediator.Verify(m => m.Send(It.Is<GetSharingByIdQuery>(q =>
            q.SharingId == sharingId &&
            q.Limit == limit), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
        Guid sharingId,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SharingController controller)
        {
            // Arrange
            mediator
            .Setup(x => x.Send(It.IsAny<GetSharingByIdQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetSharingById(sharingId, null) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetSharingByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Sharing_Is_Returned_With_Default_Null_Limit(
        Guid sharingId,
        GetSharingByIdQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SharingController controller)
        {
            // Arrange
            mediator
            .Setup(x => x.Send(It.Is<GetSharingByIdQuery>(q =>
            q.SharingId == sharingId &&
            q.Limit == null), CancellationToken.None))
            .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharingById(sharingId, null) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult.Response);

            mediator.Verify(m => m.Send(It.Is<GetSharingByIdQuery>(q =>
            q.SharingId == sharingId &&
            q.Limit == null), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
