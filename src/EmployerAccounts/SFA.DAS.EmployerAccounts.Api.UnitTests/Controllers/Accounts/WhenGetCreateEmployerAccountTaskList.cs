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
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Accounts;

public class WhenGetCreateEmployerAccountTaskList
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CreateAccountTaskList_From_Mediator(
        long accountId,
        string hashedAccountId,
        string userRef,
        GetCreateAccountTaskListQueryResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] AccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetCreateAccountTaskListQuery>(p => p.HashedAccountId.Equals(hashedAccountId) && p.AccountId.Equals(accountId) && p.UserRef.Equals(userRef)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetCreateAccountTaskList(accountId, hashedAccountId, userRef) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        
        var model = controllerResult.Value as GetCreateAccountTaskListResponse;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo((GetCreateAccountTaskListResponse)mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Then_No_EmployerAccountTaskList_Are_Returned_From_Mediator(
        long accountId,
        string hashedAccountId,
        string userRef,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] AccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetCreateAccountTaskListQuery>(p => p.HashedAccountId.Equals(hashedAccountId) && p.AccountId.Equals(accountId) && p.UserRef.Equals(userRef)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var controllerResult = await controller.GetCreateAccountTaskList(accountId, hashedAccountId, userRef) as OkObjectResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        controllerResult.Value.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Bad_Request(
        long accountId,
        string hashedAccountId,
        string userRef,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] AccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetCreateAccountTaskListQuery>(p => p.HashedAccountId.Equals(hashedAccountId) && p.AccountId.Equals(accountId) && p.UserRef.Equals(userRef)),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var controllerResult = await controller.GetCreateAccountTaskList(accountId, hashedAccountId, userRef) as BadRequestResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}