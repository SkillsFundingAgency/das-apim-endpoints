using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreditPledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreditPledge
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private CreditPledgeCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private CreditPledgeCommand _command;
        private CreditPledgeRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<CreditPledgeCommand>();

            var apiResponse = new ApiResponse<CreditPledgeRequest>(new CreditPledgeRequest(_command.PledgeId, new CreditPledgeRequest.CreditPledgeRequestData()), HttpStatusCode.OK, string.Empty);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.CreditPledge(It.IsAny<CreditPledgeRequest>()))
                .Callback<CreditPledgeRequest>(r => _request = r)
                .ReturnsAsync(apiResponse);

            _handler = new CreditPledgeCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<CreditPledgeCommandHandler>>());
        }

        [Test]
        public async Task Pledge_Is_Credited()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var debit = (CreditPledgeRequest.CreditPledgeRequestData)_request.Data;

            Assert.AreEqual($"pledges/{_command.PledgeId}/credit", _request.PostUrl);
            Assert.AreEqual(_command.Amount, debit.Amount);
            Assert.AreEqual(_command.ApplicationId, debit.ApplicationId);
        }
    }
}