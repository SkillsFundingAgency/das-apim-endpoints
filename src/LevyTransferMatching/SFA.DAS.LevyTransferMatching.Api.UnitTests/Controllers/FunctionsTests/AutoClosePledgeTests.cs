using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using MediatR;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class AutoClosePledgeTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private Fixture _fixture;
        private AutoClosePledgeCommandResult _result;
        
        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
            _fixture = new Fixture();
            _result = _fixture.Create<AutoClosePledgeCommandResult>();
        }

        [Test]
        public async Task AutoClosePledge_Returns_Response()
        {
            var request = _fixture.Create<AutoClosePledgeRequest>();

            _mediator.Setup(x => x.Send(It.IsAny<AutoClosePledgeCommand>(), CancellationToken.None))
                .ReturnsAsync(_result);

            await _controller.AutoClosePledge(request);

            _mediator.Verify(x =>
                x.Send(It.IsAny<AutoClosePledgeCommand>(),
                    It.IsAny<CancellationToken>()));
        }
    }
}