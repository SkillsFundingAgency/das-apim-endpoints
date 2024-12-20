﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Controllers;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.Application.Pledges.Queries.GetPledges;

namespace SFA.DAS.Forecasting.Api.UnitTests.Controllers.Pledges;

[TestFixture]
public class WhenGettingPledges
{
    private PledgesController _controller;
    private Mock<IMediator> _mediator;
    private readonly Fixture _fixture = new();
    private GetPledgesQueryResult _queryResult;
    private long _accountId;

    [SetUp]
    public void Setup()
    {
        _accountId = _fixture.Create<long>();

        _mediator = new Mock<IMediator>();

        _queryResult = _fixture.Create<GetPledgesQueryResult>();
        _mediator.Setup(x => x.Send(It.Is<GetPledgesQuery>(q => q.AccountId == _accountId), CancellationToken.None))
            .ReturnsAsync(_queryResult);

        _controller = new PledgesController(_mediator.Object);
    }

    [Test]
    public async Task Then_Pledges_Are_Returned_Correctly()
    {
        var result = await _controller.GetPledges(_accountId) as ObjectResult;

        Assert.That(result, Is.Not.Null);
        var response = result.Value as GetPledgesResponse;
        Assert.That(response, Is.Not.Null);

        Assert.That(_queryResult.Pledges.Count(), Is.EqualTo(response.Pledges.Count()));

        var index = 0;

        foreach (var pledge in response.Pledges)
        {
            var expected = _queryResult.Pledges.ToArray()[index];
            Assert.That(expected.Id, Is.EqualTo(pledge.Id));
            Assert.That(expected.AccountId, Is.EqualTo(pledge.AccountId));
            index++;
        }
    }
}