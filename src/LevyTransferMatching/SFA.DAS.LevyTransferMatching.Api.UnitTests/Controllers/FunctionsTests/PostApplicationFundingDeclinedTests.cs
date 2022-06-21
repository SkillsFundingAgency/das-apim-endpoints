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
    public class PostApplicationFundingDeclinedTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private ApplicationFundingDeclinedRequest _request;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _request = _fixture.Create<ApplicationFundingDeclinedRequest>();

            _mediator = new Mock<IMediator>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<Microsoft.Extensions.Logging.ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Post_ApplicationFundingDeclined_Credits_Pledge()
        {
            await _controller.ApplicationFundingDeclined(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<CreditPledgeCommand>(c => c.PledgeId == _request.PledgeId && c.Amount == _request.Amount && c.ApplicationId == _request.ApplicationId),
                    It.IsAny<CancellationToken>()));
        }
    }
}