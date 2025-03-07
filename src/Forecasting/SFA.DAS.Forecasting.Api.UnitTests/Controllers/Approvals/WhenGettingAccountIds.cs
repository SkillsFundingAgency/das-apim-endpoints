﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Approvals.Queries.GetAccountIds;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Approvals;

[TestFixture]
public class WhenGettingAccountIds
{
    private ApprovalsController _controller;
    private Mock<IMediator> _mediator;
    private readonly Fixture _fixture = new();
    private GetAccountIdsQueryResult _queryResult;

    [SetUp]
    public void Setup()
    {
        _mediator = new Mock<IMediator>();

        _queryResult = _fixture.Create<GetAccountIdsQueryResult>();
        _mediator.Setup(x => x.Send(It.IsAny<GetAccountIdsQuery>(), CancellationToken.None))
            .ReturnsAsync(_queryResult);

        _controller = new ApprovalsController(_mediator.Object, Mock.Of<ILogger<ApprovalsController>>());
    }

    [Test]
    public async Task Then_Accounts_Are_Returned_Correctly()
    {
        var result = await _controller.GetAccountsWithCohorts() as ObjectResult;

        Assert.That(result, Is.Not.Null);
        var response = result.Value as GetAccountsWithCohortsResponse;
        Assert.That(response, Is.Not.Null);

        response.AccountIds.Should().BeEquivalentTo(_queryResult.AccountIds);
    }
}