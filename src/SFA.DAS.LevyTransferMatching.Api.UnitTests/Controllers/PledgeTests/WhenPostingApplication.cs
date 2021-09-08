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
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class WhenPostingApplication
    {
        private PledgeController _controller;

        private long _accountId;
        private int _pledgeId;
        private int _applicationId;
        private ApproveApplicationRequest _approveApplicationRequest;
        private Mock<IMediator> _mediator;

        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _accountId = _fixture.Create<long>();
            _pledgeId = _fixture.Create <int>();
            _applicationId = _fixture.Create<int>();
            _approveApplicationRequest = _fixture.Create<ApproveApplicationRequest>();

            _mediator = new Mock<IMediator>();

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task Application_Is_Approved()
        {
            await _controller.Application(_accountId, _pledgeId, _applicationId, _approveApplicationRequest);

            _mediator.Verify(x => x.Send(It.Is<ApproveApplicationCommand>(c =>
                c.PledgeId == _pledgeId &&
                c.ApplicationId == _applicationId &&
                c.UserId == _approveApplicationRequest.UserId &&
                c.UserDisplayName == _approveApplicationRequest.UserDisplayName), It.IsAny<CancellationToken>()),Times.Once);
        }

        [Test]
        public async Task OkResponse_Is_Returned()
        {
            var result = await _controller.Application(_accountId, _pledgeId, _applicationId, _approveApplicationRequest);
            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}
