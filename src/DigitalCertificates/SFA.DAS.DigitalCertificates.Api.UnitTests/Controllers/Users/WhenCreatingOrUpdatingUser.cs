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
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingOrUpdatingUser
    {
        [Test, MoqAutoData]
        public async Task Then_The_UserId_Is_Returned_From_Mediator(
            CreateOrUpdateUserRequest request,
            CreateOrUpdateUserResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator
                .Setup(x => x.Send(It.Is<CreateOrUpdateUserCommand>(c =>
                    c.GovUkIdentifier == request.GovUkIdentifier &&
                    c.EmailAddress == request.EmailAddress &&
                    c.PhoneNumber == request.PhoneNumber &&
                    c.Names == request.Names &&
                    c.DateOfBirth == request.DateOfBirth), CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.CreateOrUpdateUser(request) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().Be(result.UserId);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            CreateOrUpdateUserRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] UsersController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<CreateOrUpdateUserCommand>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.CreateOrUpdateUser(request) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
