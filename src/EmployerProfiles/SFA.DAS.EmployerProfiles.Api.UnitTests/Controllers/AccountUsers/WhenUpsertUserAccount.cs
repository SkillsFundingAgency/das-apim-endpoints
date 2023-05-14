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
using SFA.DAS.EmployerProfiles.Api.Controllers;
using SFA.DAS.EmployerProfiles.Api.Models;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerProfiles.Api.UnitTests.Controllers.AccountUsers
{
    public class WhenUpsertUserAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Upsert_UserAccount_From_Mediator(
            string userId,
            UpsertAccountRequest request,
            EmployerProfile mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<UpsertAccountCommand>(c => 
                        c.Email.Equals(request.Email)
                        && c.GovIdentifier.Equals(request.GovIdentifier)
                        && c.FirstName.Equals(request.FirstName)
                        && c.LastName.Equals(request.LastName)
                        && c.UserId.Equals(userId)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.UpsertUserAccount(userId, request) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as UpsertUserApiResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((UpsertUserApiResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            string email,
            UpsertAccountRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<UpsertAccountCommand>(c => c.Email.Equals(request.Email)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.UpsertUserAccount(email, request) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}