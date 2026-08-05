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
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingCertificatesMatch
    {
        [Test, MoqAutoData]
        public async Task Then_The_Matches_Are_Returned_From_Mediator(
            Guid userId,
            GetCertificatesMatchResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetCertificatesMatchQuery>(q => q.UserId == userId), CancellationToken.None))
                .ReturnsAsync(queryResult);
            // Act
            var actual = await controller.GetCertificatesMatch(userId) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_NoContent_Is_Returned_When_User_Already_Authorised(
            Guid userId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetCertificatesMatchQuery>(q => q.UserId == userId), CancellationToken.None))
                .ReturnsAsync((GetCertificatesMatchResult)null);
            // Act
            var actual = await controller.GetCertificatesMatch(userId) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<GetCertificatesMatchQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());
            // Act
            var actual = await controller.GetCertificatesMatch(userId) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
