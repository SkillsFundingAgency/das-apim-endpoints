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
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationApprovalOptions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.PledgeTests
{
    [TestFixture]
    public class WhenCallingSetApplicationApprovalOptions
    {
        private PledgeController _controller;

        private long _accountId;
        private int _pledgeId;
        private int _applicationId;
        private SetApplicationApprovalOptionsRequest _request;
        private Mock<IMediator> _mediator;

        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _accountId = _fixture.Create<long>();
            _pledgeId = _fixture.Create<int>();
            _applicationId = _fixture.Create<int>();
            _request = _fixture.Create<SetApplicationApprovalOptionsRequest>();

            _mediator = new Mock<IMediator>();

            _controller = new PledgeController(_mediator.Object, Mock.Of<ILogger<PledgeController>>());
        }

        [Test]
        public async Task Application_Outcome_Is_Set()
        {
            await _controller.SetApplicationApprovalOptions(_pledgeId, _applicationId, _request);

            _mediator.Verify(x => x.Send(It.Is<SetApplicationApprovalOptionsCommand>(c =>
                c.PledgeId == _pledgeId &&
                c.ApplicationId == _applicationId &&
                c.AutomaticApproval == _request.AutomaticApproval &&
                c.UserId == _request.UserId &&
                c.UserDisplayName == _request.UserDisplayName), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task OkResponse_Is_Returned()
        {
            var result = await _controller.SetApplicationApprovalOptions(_pledgeId, _applicationId, _request);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }
    }
}