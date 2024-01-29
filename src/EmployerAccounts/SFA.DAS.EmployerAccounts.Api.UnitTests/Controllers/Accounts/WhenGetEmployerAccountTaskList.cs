using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEmployerAccountTaskList;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Accounts
{
    public class WhenGetEmployerAccountTaskList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_EmployerAccountTaskList_From_Mediator(
            long accountId,
            string hashedAccountId,
            GetEmployerAccountTaskListQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetEmployerAccountTaskListQuery>(p => p.HashedAccountId == hashedAccountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetEmployerAccountTaskList(accountId, hashedAccountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetEmployerAccountTaskListResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((GetEmployerAccountTaskListResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Then_No_EmployerAccountTaskList_Are_Returned_From_Mediator(
            long accountId,
            string hashedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetEmployerAccountTaskListQuery>(p => p.HashedAccountId == hashedAccountId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var controllerResult = await controller.GetEmployerAccountTaskList(accountId, hashedAccountId) as NotFoundResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            string hashedAccountId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetEmployerAccountTaskListQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetEmployerAccountTaskList(accountId, hashedAccountId) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
