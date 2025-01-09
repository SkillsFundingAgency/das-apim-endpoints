using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class PostApplicationFundingExpiredTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private ApplicationFundingExpiredRequest _request;
        private readonly Fixture _fixture = new();

        [SetUp]
        public void Setup()
        {
            _request = _fixture.Create<ApplicationFundingExpiredRequest>();

            _mediator = new Mock<IMediator>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Post_ApplicationFundingExpired_Credits_Pledge()
        {
            await _controller.ApplicationFundingExpired(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<CreditPledgeCommand>(c => 
                    c.PledgeId == _request.PledgeId 
                    && c.Amount == _request.Amount 
                    && c.ApplicationId == _request.ApplicationId),
                    It.IsAny<CancellationToken>()));
        }
    }
}