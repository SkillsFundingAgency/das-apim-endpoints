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
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery;
using SFA.DAS.Approvals.Application.Apprentices.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Accounts
{
    public class WhenGettingAnAccount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long accountId,
            GetAccountResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountQuery>(x => x.AccountId == accountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.Get(accountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountResult;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_Account_Is_Returned_From_Mediator(
            long accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            var controllerResult = await controller.Get(accountId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int) HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.Get(accountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}