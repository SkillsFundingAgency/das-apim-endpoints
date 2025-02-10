using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingUsersByEmail
{
    [Test, MoqAutoData]
    public async Task Then_UsersQueryResponse_Returned_From_Mediator(
        string email,
        GetUsersByEmailQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] UsersQueryController sut)
    {
        mockMediator.Setup(x => x.Send(It.Is<GetUsersByEmailQuery>(p=>p.Email == email), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(email) as ObjectResult;
        var actualValue = actual!.Value as GetUsersByEmailQueryResult;

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<GetUsersByEmailQueryResult>()
                .Which.Users.Should().BeEquivalentTo(mockQueryResult.Users);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        string email,
        GetUsersByEmailQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] UsersQueryController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetUsersByEmailQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(email) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}