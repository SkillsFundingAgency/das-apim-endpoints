using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationApprovalOptions;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.SetApplicationApprovalOptions
{
    public class WhenCallingHandle
    {
        private SetApplicationApprovalOptionsCommandHandler _handler;
        private Fixture _fixture = new Fixture();
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;

        private SetApplicationApprovalOptionsCommand _command;
        private ApproveApplicationRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<SetApplicationApprovalOptionsCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.ApproveApplication(It.Is<ApproveApplicationRequest>(x => x.ApplicationId == _command.ApplicationId)))
                .Callback<ApproveApplicationRequest>(request => _request = request)
                .Returns(Task.CompletedTask);

            _handler = new SetApplicationApprovalOptionsCommandHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Application_Is_Approved()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var requestData = (ApproveApplicationRequestData)_request.Data;

            _levyTransferMatchingService.Verify(
                x => x.ApproveApplication(It.Is<ApproveApplicationRequest>(r =>
                    r.PledgeId == _command.PledgeId &&
                    r.ApplicationId == _command.ApplicationId &&
                    requestData.UserId == _command.UserId &&
                    requestData.UserDisplayName == _command.UserDisplayName)),
                Times.Once);
        }
    }
}