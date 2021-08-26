using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DebitPledge
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private DebitPledgeCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private DebitPledgeCommand _command;
        private DebitPledgeRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<DebitPledgeCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.DebitPledge(It.IsAny<DebitPledgeRequest>()))
                .Callback<DebitPledgeRequest>(r => _request = r)
                .Returns(Task.CompletedTask);
            
            _handler = new DebitPledgeCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<DebitPledgeCommandHandler>>());
        }

        [Test]
        public async Task Pledge_Is_Debited()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var debit = (DebitPledgeRequest.DebitPledgeRequestData) _request.Data;

            Assert.AreEqual($"pledges/{_command.PledgeId}/debit", _request.PostUrl);
            Assert.AreEqual(_command.Amount, debit.Amount);
        }
    }
}
