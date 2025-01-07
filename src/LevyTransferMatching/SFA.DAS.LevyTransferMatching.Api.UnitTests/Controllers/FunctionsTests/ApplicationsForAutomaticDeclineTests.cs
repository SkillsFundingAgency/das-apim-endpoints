using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests;
[TestFixture]
public class ApplicationsForAutomaticDeclineTests
{
    private FunctionsController _controller;
    private Mock<IMediator> _mediator;

    [SetUp]
    public void Setup()
    {
        _mediator = new Mock<IMediator>();
        _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
    }

    [Test]
    public async Task Action_Calls_Handler()
    {
        // Arrange
        var fixture = new Fixture();
        var expectedResult = fixture.Create<ApplicationsForAutomaticDeclineResult>();
        _mediator.Setup(x => x.Send(It.IsAny<ApplicationsForAutomaticDeclineQuery>(), default))
            .ReturnsAsync(expectedResult);

        await _controller.ApplicationsForAutomaticDecline();

        _mediator.Verify(x =>
            x.Send(It.IsAny<ApplicationsForAutomaticDeclineQuery>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task ApplicationsForAutomaticDecline_WhenQuerySucceeds_ReturnsOkResultWithData()
    {
        // Arrange
        var fixture = new Fixture();
        var expectedResult = fixture.Create<ApplicationsForAutomaticDeclineResult>();
        _mediator.Setup(x => x.Send(It.IsAny<ApplicationsForAutomaticDeclineQuery>(), default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.ApplicationsForAutomaticDecline();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult.Value as ApplicationsForAutomaticDeclineResponse;
        response.Should().NotBeNull();
        response.ApplicationIdsToDecline.Count().Should().Be(expectedResult.ApplicationIdsToDecline.Count());
    }
}
