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
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharings;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingSharings
    {
        [Test, MoqAutoData]
        public async Task Then_The_Sharings_Are_Returned(
            Guid userId,
            Guid certificateId,
            int limit,
            GetSharingsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetSharingsQuery>(q =>
                    q.UserId == userId &&
                    q.CertificateId == certificateId &&
                    q.Limit == limit), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharings(userId, certificateId, limit) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult.Response);

            mediator.Verify(m => m.Send(It.Is<GetSharingsQuery>(q =>
                q.UserId == userId &&
                q.CertificateId == certificateId &&
                q.Limit == limit), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            Guid certificateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<GetSharingsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetSharings(userId, certificateId) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetSharingsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Sharings_Are_Returned_With_Default_Limit_When_Limit_Is_Null(
            Guid userId,
            Guid certificateId,
            GetSharingsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetSharingsQuery>(q =>
                    q.UserId == userId &&
                    q.CertificateId == certificateId &&
                    q.Limit == null), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetSharings(userId, certificateId, null) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult.Response);

            mediator.Verify(m => m.Send(It.Is<GetSharingsQuery>(q =>
                q.UserId == userId &&
                q.CertificateId == certificateId &&
                q.Limit == null), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}