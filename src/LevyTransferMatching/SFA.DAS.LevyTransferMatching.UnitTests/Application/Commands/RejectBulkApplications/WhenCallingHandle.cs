using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.RejectBulkApplications
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private RejectApplicationsCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private RejectApplicationsCommand _command;
        private RejectApplicationRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<RejectApplicationsCommand>();
            _command.ApplicationsToReject = new List<int> {5};

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.RejectApplication(It.IsAny<RejectApplicationRequest>()))
                .Callback<RejectApplicationRequest>(r => _request = r)
                .Returns(Task.CompletedTask);

            _handler = new RejectApplicationsCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<RejectApplicationsCommandHandler>>());
        }

        [Test]
        public async Task Bulk_Applications_Rejected_Successfully()
        {
            var response = await _handler.Handle(_command, CancellationToken.None);

            var data = (RejectApplicationRequestData)_request.Data;
            var applicationID = _command.ApplicationsToReject[0];
            Assert.AreEqual($"/pledges/{_command.PledgeId}/applications/{applicationID}/reject", _request.PostUrl);
            Assert.AreEqual(_command.UserId, data.UserId);
            Assert.AreEqual(_command.UserDisplayName, data.UserDisplayName);
            
            _levyTransferMatchingService.Verify(
               x => x.RejectApplication(It.Is<RejectApplicationRequest>(r =>
                   r.PledgeId == _command.PledgeId &&
                   r.ApplicationId == applicationID &&
                   data.UserId == _command.UserId &&
                   data.UserDisplayName == _command.UserDisplayName)),
               Times.Once);
        }
    }
}
