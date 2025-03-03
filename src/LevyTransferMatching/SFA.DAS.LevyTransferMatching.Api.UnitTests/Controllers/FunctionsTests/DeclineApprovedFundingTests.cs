using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.DeclineApprovedFunding;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests;

public class DeclineApprovedFundingTests
{
    private FunctionsController _controller;
    private Mock<IMediator> _mediator;
    private readonly Fixture _fixture = new();
    private DeclineApprovedFundingRequest _request;

    [SetUp]
    public void Setup()
    {
        _mediator = new Mock<IMediator>();
        _request = _fixture.Create<DeclineApprovedFundingRequest>();

        _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
    }

    [Test]
    public async Task Action_Calls_Handler()
    {
        await _controller.DeclineApprovedFunding(_request);

        _mediator.Verify(x =>
            x.Send(It.Is<DeclineApprovedFundingCommand>(c =>
            c.ApplicationId == _request.ApplicationId), It.IsAny<CancellationToken>()));
    }
}
