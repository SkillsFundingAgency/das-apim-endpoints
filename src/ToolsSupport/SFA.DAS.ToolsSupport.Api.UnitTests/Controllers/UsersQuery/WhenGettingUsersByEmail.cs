using System.Net;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers.UsersQuery;

[TestFixture]
public class WhenGettingUsersByEmail
{
    [Test, MoqAutoData]
    public async Task Then_UsersQueryResponse_Returned_From_Mediator(
        string email,
        GetUsersByEmailQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] UsersController sut)
    {
        mockMediator.Setup(x => x.Send(It.Is<GetUsersByEmailQuery>(p => p.Email == email), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(email) as ObjectResult;

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
        [Greedy] UsersController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetUsersByEmailQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(email) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}