using System.Net;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingEmployerAccounts
{
    [Test, MoqAutoData]
    public async Task Then_EmployerAccountsResponse_Returned_From_Mediator(
        long accountId,
        string payRef,
        SearchEmployerAccountsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController sut)
    {
        mockMediator.Setup(x => x.Send(It.Is<SearchEmployerAccountsQuery>(p=>p.AccountId == accountId && p.PayeSchemeRef == payRef), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(accountId, payRef, null) as ObjectResult;

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<SearchEmployerAccountsQueryResult>()
                .Which.EmployerAccounts.Should().BeEquivalentTo(mockQueryResult.EmployerAccounts);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        long accountId,
        string payRef,
        SearchEmployerAccountsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<SearchEmployerAccountsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(accountId, payRef, null) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}