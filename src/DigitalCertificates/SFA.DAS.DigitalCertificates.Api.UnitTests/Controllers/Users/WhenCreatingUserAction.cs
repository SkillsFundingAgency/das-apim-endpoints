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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingUserAction
    {
        [Test, MoqAutoData]
        public async Task Then_The_ActionCode_Is_Returned(
            Guid userId,
            Models.Users.CreateUserActionRequest request,
            CreateUserActionResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator
                .Setup(x => x.Send(It.Is<CreateUserActionCommand>(c =>
                    c.UserId == userId &&
                    c.ActionType == request.ActionType &&
                    c.FamilyName == request.FamilyName &&
                    c.GivenNames == request.GivenNames &&
                    c.CertificateId == request.CertificateId &&
                    c.CertificateType == request.CertificateType &&
                    c.CourseName == request.CourseName), CancellationToken.None))
                .ReturnsAsync(result);

            // Act
            var actual = await controller.CreateUserAction(userId, request) as ObjectResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var expected = (Models.Users.CreateUserActionResponse)result;
            actual.Value.Should().BeEquivalentTo(expected);

            mediator.Verify(m => m.Send(It.Is<CreateUserActionCommand>(c =>
                c.UserId == userId &&
                c.ActionType == request.ActionType), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            Models.Users.CreateUserActionRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<CreateUserActionCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            // Act
            var actual = await controller.CreateUserAction(userId, request) as StatusCodeResult;

            // Assert
            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.IsAny<CreateUserActionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
