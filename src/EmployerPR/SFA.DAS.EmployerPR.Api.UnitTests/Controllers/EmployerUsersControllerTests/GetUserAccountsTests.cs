using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Api.Controllers;
using SFA.DAS.EmployerPR.Application.UserAccounts.Queries.GetUserAccounts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.Api.UnitTests.Controllers.EmployerUsersControllerTests;

public class GetUserAccountsTests
{
    [Test, MoqAutoData]
    public async Task GetUserAccounts_InvokesMediator_ReturnsMediatorResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerUsersController sut,
        string userId,
        string email,
        GetUserAccountsQueryResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(e => e.Send(It.Is<GetUserAccountsQuery>(p => p.UserId == userId && p.Email == email), cancellationToken)).ReturnsAsync(expected);

        var result = await sut.GetUserAccounts(userId, email, cancellationToken);

        mediatorMock.Verify(e => e.Send(It.Is<GetUserAccountsQuery>(p => p.UserId == userId && p.Email == email), cancellationToken), Times.Once);
        result.As<OkObjectResult>().Value.Should().Be(expected);
    }
}
