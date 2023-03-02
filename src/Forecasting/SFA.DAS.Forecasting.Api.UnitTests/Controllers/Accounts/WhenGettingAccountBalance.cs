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
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Accounts
{
    public class WhenGettingAccountBalance
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_UserAccounts_From_Mediator(
            string accountId,
            GetAccountBalanceQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetAccountBalanceQuery>(c => c.AccountId.Equals(accountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAccountBalance(accountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetAccountBalanceApiResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetAccountBalanceApiResponse)mediatorResult);
        }
        
        [Test, MoqAutoData]
        public async Task And_Empty_Result_Then_Returns_NotFound_Result(
            string accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountBalanceQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAccountBalanceQueryResult
                {
                    AccountBalance = null
                });

            var controllerResult = await controller.GetAccountBalance(accountId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Internal_Server_Error(
            string accountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetAccountBalanceQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.GetAccountBalance(accountId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}