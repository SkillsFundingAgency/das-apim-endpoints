using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private RejectApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();
        private RejectApplicationCommand _command;
        private RejectApplicationRequest _rejectApplicationRequest;
     
        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<RejectApplicationCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()))
                .Callback<RejectApplicationRequest>(request => _rejectApplicationRequest = request)
                .Returns(Task.CompletedTask);          

            _handler = new RejectApplicationCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<RejectApplicationCommandHandler>>());
        }

        [Test]
        public async Task Application_Is_Rejected()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var requestData = (RejectApplicationRequestData) _rejectApplicationRequest.Data;

            _levyTransferMatchingService.Verify(
                x => x.RejectApplication(It.Is<RejectApplicationRequest>(r =>
                    r.PledgeId == _command.PledgeId &&
                    r.ApplicationId == _command.ApplicationId
                     && r.Data == requestData
                 )),
                Times.Once);
        }
    }
}
