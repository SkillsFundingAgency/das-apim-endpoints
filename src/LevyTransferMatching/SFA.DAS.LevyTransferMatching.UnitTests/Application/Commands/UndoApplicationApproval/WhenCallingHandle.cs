using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.UndoApplicationApproval
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private UndoApplicationApprovalCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private UndoApplicationApprovalCommand _command;
        private UndoApplicationApprovalRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<UndoApplicationApprovalCommand>();

            var apiResponse = new ApiResponse<UndoApplicationApprovalRequest>(new UndoApplicationApprovalRequest(_command.PledgeId, _command.ApplicationId), HttpStatusCode.OK, string.Empty);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.UndoApplicationApproval(It.IsAny<UndoApplicationApprovalRequest>()))
                .Callback<UndoApplicationApprovalRequest>(r => _request = r)
                .ReturnsAsync(apiResponse);
            
            _handler = new UndoApplicationApprovalCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<UndoApplicationApprovalCommandHandler>>());
        }

        [Test]
        public async Task Approval_Is_Undone()
        {
            await _handler.Handle(_command, CancellationToken.None);

            Assert.That(_request.PostUrl, Is.EqualTo($"pledges/{_command.PledgeId}/applications/{_command.ApplicationId}/undo-approval"));
        }
    }
}
