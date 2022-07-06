using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationWithdrawnAfterAcceptance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class ApplicationWithdrawnAfterAcceptanceTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private ApplicationWithdrawnAfterAcceptanceRequest _request;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _request = _fixture.Create<ApplicationWithdrawnAfterAcceptanceRequest>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Action_Calls_Handler()
        {
            var result = await _controller.ApplicationWithdrawnAfterAcceptance(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<ApplicationWithdrawnAfterAcceptanceCommand>(c => c.ApplicationId == _request.ApplicationId &&
                c.PledgeId == _request.PledgeId &&
                c.Amount == _request.Amount), It.IsAny<CancellationToken>()));
        }
    }
}
