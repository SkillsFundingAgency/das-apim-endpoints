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
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenUpdatingUserIdentity
    {
        [Test, MoqAutoData]
        public async Task Then_Ok_Is_Returned_And_Command_Is_Sent(
            Guid userId,
            UpdateUserIdentityRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator
                .Setup(x => x.Send(It.Is<UpdateUserIdentityCommand>(c =>
                    c.UserId == userId &&
                    c.Names == request.Names &&
                    c.DateOfBirth == request.DateOfBirth), CancellationToken.None))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.UpdateUserIdentity(userId, request) as OkResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);

            mediator.Verify(x => x.Send(It.Is<UpdateUserIdentityCommand>(c =>
                c.UserId == userId &&
                c.Names == request.Names &&
                c.DateOfBirth == request.DateOfBirth), CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid userId,
            UpdateUserIdentityRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator
                .Setup(x => x.Send(It.IsAny<UpdateUserIdentityCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.UpdateUserIdentity(userId, request) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}