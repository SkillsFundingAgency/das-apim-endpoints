using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ProviderRelationships.Api.Controllers;
using SFA.DAS.ProviderRelationships.Api.Models;
using SFA.DAS.ProviderRelationships.Application.AccountUsers.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRelationships.Api.UnitTests.Controllers.AccountUsers;

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

        Assert.That(controllerResult, Is.Not.Null);
        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetUserAccountsApiResponse;
        Assert.That(model, Is.Not.Null);
        model.Should().BeEquivalentTo((GetUserAccountsApiResponse)mediatorResult);
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

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}