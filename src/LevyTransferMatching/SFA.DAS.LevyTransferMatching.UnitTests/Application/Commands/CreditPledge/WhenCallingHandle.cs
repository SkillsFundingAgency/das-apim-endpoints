﻿using System.Net;
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
        private readonly Fixture _fixture = new Fixture();

        private CreditPledgeCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;

        [SetUp]
        public void Setup()
        {
            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _handler = new CreditPledgeCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<CreditPledgeCommandHandler>>());
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public async Task Amount_Is_Greater_Than_Zero_Pledge_Is_Credited(int amount)
        {
            var command = _fixture
                .Build<CreditPledgeCommand>()
                .With(x => x.Amount, amount)
                .Create();
            
            var apiResponse = new ApiResponse<CreditPledgeRequest>(new CreditPledgeRequest(command.PledgeId, new CreditPledgeRequest.CreditPledgeRequestData()), HttpStatusCode.OK, string.Empty);

            CreditPledgeRequest request = null;
            _levyTransferMatchingService.Setup(x => x.CreditPledge(It.IsAny<CreditPledgeRequest>()))
                .Callback<CreditPledgeRequest>(r => request = r)
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(command, CancellationToken.None);

            var credit = (CreditPledgeRequest.CreditPledgeRequestData)request.Data;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CreditPledgeSkipped, Is.False);
            Assert.That(request.PostUrl, Is.EqualTo($"pledges/{command.PledgeId}/credit"));
            Assert.That(command.Amount, Is.EqualTo(credit.Amount));
            Assert.That(command.ApplicationId, Is.EqualTo(credit.ApplicationId));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        public async Task Amount_Is_Equal_To_Or_Less_Than_Zero_Pledge_Isnt_Credited(int amount)
        {
            var command = _fixture
                .Build<CreditPledgeCommand>()
                .With(x => x.Amount, amount)
                .Create();

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CreditPledgeSkipped, Is.True);
        }
    }
}