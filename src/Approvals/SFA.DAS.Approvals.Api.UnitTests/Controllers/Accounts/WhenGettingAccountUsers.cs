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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Accounts
{
    public class WhenGettingAccountUsers
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_AccountUsers_From_Mediator(
            string hashedAccountId,
            GetAccountUsersResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountUsersQuery>(x => x.HashedAccountId == hashedAccountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetUsers(hashedAccountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountUsersResult;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_AccountUsers_Are_Returned_From_Mediator(
            string hashedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountUsersQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var controllerResult = await controller.GetUsers(hashedAccountId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string hashedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountUsersQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetUsers(hashedAccountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}