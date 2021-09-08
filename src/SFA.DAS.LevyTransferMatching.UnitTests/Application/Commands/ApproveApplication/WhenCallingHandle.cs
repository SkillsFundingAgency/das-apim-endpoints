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
        private ApproveApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Fixture _fixture = new Fixture();

        private ApproveApplicationCommand _command;
        private ApproveApplicationRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<ApproveApplicationCommand>();

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.ApproveApplication(It.IsAny<ApproveApplicationRequest>()))
                .Callback<ApproveApplicationRequest>(request => _request = request)
                .Returns(Task.CompletedTask);

            _handler = new ApproveApplicationCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<ApproveApplicationCommandHandler>>());
        }


        [Test]
        public async Task Application_Is_Approved()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var requestData = (ApproveApplicationRequestData) _request.Data;

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
