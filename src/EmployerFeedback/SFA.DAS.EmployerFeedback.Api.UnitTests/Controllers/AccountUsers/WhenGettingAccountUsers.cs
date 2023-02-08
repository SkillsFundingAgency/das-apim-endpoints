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
using SFA.DAS.EmployerFeedback.Api.Controllers;
using SFA.DAS.EmployerFeedback.Api.Models;
using SFA.DAS.EmployerFeedback.Application.AccountUsers.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerFeedback.Api.UnitTests.Controllers.AccountUsers
{
    public class WhenGettingUserAccounts
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_UserAccounts_From_Mediator(
            string userId,
            string email,
            GetAccountsQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountsQuery>(c => c.UserId.Equals(userId) && c.Email.Equals(email)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetUserAccounts(userId,email) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as UserAccountsApiResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((UserAccountsApiResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            string userId,
            string email,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.GetUserAccounts(userId, email) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}