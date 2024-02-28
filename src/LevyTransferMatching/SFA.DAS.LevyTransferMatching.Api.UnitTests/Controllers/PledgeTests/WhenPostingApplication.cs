using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Pledges;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class WhenPostingApplication
    {
        private PledgeController _controller;

        private long _accountId;
        private int _pledgeId;
        private int _applicationId;
        private SetApplicationOutcomeRequest _setApplicationOutcomeRequest;
        private Mock<IMediator> _mediator;

        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _accountId = _fixture.Create<long>();
            _pledgeId = _fixture.Create <int>();
            _applicationId = _fixture.Create<int>();
            _setApplicationOutcomeRequest = _fixture.Create<SetApplicationOutcomeRequest>();

            _mediator = new Mock<IMediator>();

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task Application_Outcome_Is_Set()
        {
            await _controller.Application(_accountId, _pledgeId, _applicationId, _setApplicationOutcomeRequest);

            _mediator.Verify(x => x.Send(It.Is<SetApplicationOutcomeCommand>(c =>
                c.Outcome == _setApplicationOutcomeRequest.Outcome &&
                c.PledgeId == _pledgeId &&
                c.ApplicationId == _applicationId &&
                c.UserId == _setApplicationOutcomeRequest.UserId &&
                c.UserDisplayName == _setApplicationOutcomeRequest.UserDisplayName), It.IsAny<CancellationToken>()),Times.Once);
        }

        [Test]
        public async Task OkResponse_Is_Returned()
        {
            var result = await _controller.Application(_accountId, _pledgeId, _applicationId, _setApplicationOutcomeRequest);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }
    }
}
