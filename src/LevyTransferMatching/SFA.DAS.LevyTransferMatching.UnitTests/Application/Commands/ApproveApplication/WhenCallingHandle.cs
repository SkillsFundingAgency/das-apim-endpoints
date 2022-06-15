using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ApproveApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private SetApplicationOutcomeCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Fixture _fixture = new Fixture();

        private SetApplicationOutcomeCommand _command;
        private ApproveApplicationRequest _approveApplicationRequest;
        private RejectApplicationRequest _rejectApplicationRequest;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<SetApplicationOutcomeCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.ApproveApplication(It.IsAny<ApproveApplicationRequest>()))
                .Callback<ApproveApplicationRequest>(request => _approveApplicationRequest = request)
                .Returns(Task.CompletedTask);

            _levyTransferMatchingService.Setup(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()))
                .Callback<RejectApplicationRequest>(request => _rejectApplicationRequest = request)
                .Returns(Task.CompletedTask);

            _handler = new SetApplicationOutcomeCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<SetApplicationOutcomeCommandHandler>>());
        }


        [Test]
        public async Task Application_Is_Approved()
        {
            _command.Outcome = Types.ApplicationOutcome.Approve;
            await _handler.Handle(_command, CancellationToken.None);

            var requestData = (ApproveApplicationRequestData) _approveApplicationRequest.Data;

            _levyTransferMatchingService.Verify(
                x => x.ApproveApplication(It.Is<ApproveApplicationRequest>(r =>
                    r.PledgeId == _command.PledgeId &&
                    r.ApplicationId == _command.ApplicationId &&
                    requestData.UserId == _command.UserId &&
                    requestData.UserDisplayName == _command.UserDisplayName)),
                Times.Once);
        }

        [Test]
        public async Task Application_Is_Rejected()
        {
            _command.Outcome = Types.ApplicationOutcome.Reject;
            await _handler.Handle(_command, CancellationToken.None);

            var requestData = (RejectApplicationRequestData)_rejectApplicationRequest.Data;

            _levyTransferMatchingService.Verify(
                x => x.RejectApplication(It.Is<RejectApplicationRequest>(r =>
                    r.PledgeId == _command.PledgeId &&
                    r.ApplicationId == _command.ApplicationId &&
                    requestData.UserId == _command.UserId &&
                    requestData.UserDisplayName == _command.UserDisplayName)),
                Times.Once);
        }
    }
}
