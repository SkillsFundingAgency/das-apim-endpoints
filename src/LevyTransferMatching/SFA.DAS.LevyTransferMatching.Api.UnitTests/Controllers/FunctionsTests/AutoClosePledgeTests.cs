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

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class AutoClosePledgeTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private Fixture _fixture;
        
        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();
            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
            _fixture = new Fixture();   
        }
        
        [Test]
        public async Task AutoClosePledge_ReturnsOk_WhenCommandIsSuccessful()
        {
            // Arrange
            var request = _fixture.Create<AutoClosePledgeRequest>();

            _mediator.Setup(x => x.Send(It.IsAny<AutoClosePledgeCommand>(), CancellationToken.None))
                .ReturnsAsync(HttpStatusCode.OK);

            // Act
            var result = await _controller.AutoClosePledge(request);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task AutoClosePledge_ReturnsNotFound_WhenCommandIsNotSuccessful()
        {
            // Arrange
            var request = _fixture.Create<AutoClosePledgeRequest>();

            _mediator.Setup(x => x.Send(It.IsAny<AutoClosePledgeCommand>(), CancellationToken.None))
                .ReturnsAsync(HttpStatusCode.NotFound);

            // Act
            var result = await _controller.AutoClosePledge(request);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}