using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory;
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Accounts
{
    public class WhenGetEnglishFractionHistory
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_EnglishFractionHistory_From_Mediator(
            string hashedAccountId,
            string empRef,
            GetEnglishFractionHistoryQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetEnglishFractionHistoryQuery>(p => p.HashedAccountId == hashedAccountId && p.EmpRef == empRef),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetEnglishFractionHistory(hashedAccountId, empRef) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetEnglishFractionResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetEnglishFractionResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_EnglishFractionHistory_Are_Returned_From_Mediator(
            string hashedAccountId,
            string empRef,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            var controllerResult = await controller.GetEnglishFractionHistory(hashedAccountId, empRef) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            string hashedAccountId,
            string empRef,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEnglishFractionHistoryQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetEnglishFractionHistory(hashedAccountId, empRef) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
