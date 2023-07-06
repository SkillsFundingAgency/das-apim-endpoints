﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApproveAutomaticApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ApproveAutomaticApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private ApproveAutomaticApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Fixture _fixture = new Fixture();

        private ApproveAutomaticApplicationCommand _command;
        private ApproveApplicationRequest _approveApplicationRequest;
     

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<ApproveAutomaticApplicationCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.ApproveApplication(It.IsAny<ApproveApplicationRequest>()))
                .Callback<ApproveApplicationRequest>(request => _approveApplicationRequest = request)
                .Returns(Task.CompletedTask);          

            _handler = new ApproveAutomaticApplicationCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<ApproveAutomaticApplicationCommandHandler>>());
        }


        [Test]
        public async Task Application_Is_Approved()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var requestData = (ApproveApplicationRequestData) _approveApplicationRequest.Data;

            _levyTransferMatchingService.Verify(
                x => x.ApproveApplication(It.Is<ApproveApplicationRequest>(r =>
                    r.PledgeId == _command.PledgeId &&
                    r.ApplicationId == _command.ApplicationId
                 )),
                Times.Once);
        }

      
    }
}
