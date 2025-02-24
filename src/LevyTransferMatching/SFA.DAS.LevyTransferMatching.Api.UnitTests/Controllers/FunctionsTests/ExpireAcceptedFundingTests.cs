using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests;

public class ExpireAcceptedFundingTests
{
    private FunctionsController _controller;
    private Mock<IMediator> _mediator;
    private readonly Fixture _fixture = new();
    private ExpireAcceptedFundingRequest _request;

    [SetUp]
    public void Setup()
    {
        _mediator = new Mock<IMediator>();
        _request = _fixture.Create<ExpireAcceptedFundingRequest>();

        _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
    }

    [Test]
    public async Task Action_Calls_Handler()
    {
        await _controller.ExpireAcceptedFunding(_request);

        _mediator.Verify(x =>
            x.Send(It.Is<ExpireAcceptedFundingCommand>(c =>
            c.ApplicationId == _request.ApplicationId), It.IsAny<CancellationToken>()));
    }
}
