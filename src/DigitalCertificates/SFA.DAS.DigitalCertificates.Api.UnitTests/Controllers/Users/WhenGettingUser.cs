using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Api.Controllers;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_User_Is_Returned_From_Mediator(
            string govUkIdentifier,
            GetUserResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mockMediator
                .Setup(x => x.Send(It.Is<GetUserQuery>(p => p.GovUkIdentifier == govUkIdentifier), CancellationToken.None))
                .ReturnsAsync(queryResult);

            // Act
            var actual = await controller.GetUser(govUkIdentifier) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(queryResult.User);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            string govUkIdentifier,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<GetUserQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.GetUser(govUkIdentifier) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
