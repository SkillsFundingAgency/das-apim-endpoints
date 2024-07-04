using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.Application.Accounts.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.Accounts;

public class WhenGettingAccountUsers
{
    [Test, MoqAutoData]
    public async Task Then_Gets_AccountUsers_From_Mediator(
        long accountId,
        GetAccountUsersQueryResult result,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] AccountsController sut)
    {
        mockMediator
            .Setup(m => m.Send(
                It.Is<GetAccountUsersQuery>(c => c.AccountId == accountId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        
        var controllerResult = await sut.GetAccountUsers(accountId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetAccountUsersApiResponse;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo((GetAccountUsersApiResponse)result);
    }
}