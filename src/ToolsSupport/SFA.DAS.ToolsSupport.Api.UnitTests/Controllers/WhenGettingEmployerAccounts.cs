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
using SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccounts;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers;

[TestFixture]
public class WhenGettingEmployerAccounts
{
    [Test, MoqAutoData]
    public async Task Then_EmployerAccountsResponse_Returned_From_Mediator(
        long accountId,
        string payRef,
        GetEmployerAccountsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController sut)
    {
        mockMediator.Setup(x => x.Send(It.Is<GetEmployerAccountsQuery>(p=>p.AccountId == accountId && p.PayeSchemeRef == payRef), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

        var actual = await sut.Get(accountId, payRef, null) as ObjectResult;

        using (new AssertionScope())
        {
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeOfType<GetEmployerAccountsQueryResult>()
                .Which.Accounts.Should().BeEquivalentTo(mockQueryResult.Accounts);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
        long accountId,
        string payRef,
        GetEmployerAccountsQueryResult mockQueryResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerAccountsController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<GetEmployerAccountsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        var actual = await sut.Get(accountId, payRef, null) as StatusCodeResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}