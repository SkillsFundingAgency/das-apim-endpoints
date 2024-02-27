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
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Commands.AddProviderDetailsFromInvitation;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.AccountUsers
{
    public class WhenAddingProviderDetailsFromInvitation
    {
        [Test, MoqAutoData]
        public async Task Then_Posts_AddProviderDetailsFromInvitation_Using_Mediator(
            string userId,
            AddProviderDetailsPostRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<AddProviderDetailsFromInvitationCommand>(c =>
                        c.UserId.Equals(userId)
                        && c.Email.Equals(request.Email)
                        && c.CorrelationId.Equals(request.CorrelationId)
                        && c.AccountId.Equals(request.AccountId)
                        && c.FirstName.Equals(request.FirstName)
                        && c.LastName.Equals(request.LastName)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var controllerResult = await controller.AddProviderDetailsFromInvitation(userId, request) as OkResult;

            controllerResult.Should().NotBeNull();
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
           string userId,
           AddProviderDetailsPostRequest request,
           [Frozen] Mock<IMediator> mockMediator,
           [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<AddProviderDetailsFromInvitationCommand>(c =>
                        c.UserId.Equals(userId)
                        && c.Email.Equals(request.Email)
                        && c.CorrelationId.Equals(request.CorrelationId)
                        && c.AccountId.Equals(request.AccountId)
                        && c.FirstName.Equals(request.FirstName)
                        && c.LastName.Equals(request.LastName)
                        ),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.AddProviderDetailsFromInvitation(userId, request) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
