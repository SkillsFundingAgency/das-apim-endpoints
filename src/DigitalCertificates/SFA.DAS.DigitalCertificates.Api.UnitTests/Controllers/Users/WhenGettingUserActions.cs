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
using SFA.DAS.DigitalCertificates.Application.Queries.GetUserActions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUserActions
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserActions_Are_Returned(
            Guid userId,
            GetUserActionsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<GetUserActionsQuery>(q => q.UserId == userId), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetUserActions(userId) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(new { useractions = queryResult.UserActions });

            mediator.Verify(m => m.Send(It.Is<GetUserActionsQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.IsAny<GetUserActionsQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetUserActions(userId) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<GetUserActionsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
