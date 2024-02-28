using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ClosePledge
{

    [TestFixture]
    public class WhenCallingHandle
    {
        private ClosePledgeCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private ClosePledgeCommand _command;
        private ClosePledgeRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<ClosePledgeCommand>();

            var apiResponse = new ApiResponse<ClosePledgeRequest>(new ClosePledgeRequest(_command.PledgeId, 
                new ClosePledgeRequest.ClosePledgeRequestData()), HttpStatusCode.OK, string.Empty);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()))
                .Callback<ClosePledgeRequest>(r => _request = r)
                .ReturnsAsync(apiResponse);

            _handler = new ClosePledgeCommandHandler(_levyTransferMatchingService.Object, Mock.Of<ILogger<ClosePledgeCommandHandler>>());
        }

        [Test]
        public async Task Pledge_Is_Closed_Successfully()
        {
            var response = await _handler.Handle(_command, CancellationToken.None);

            var data = (ClosePledgeRequest.ClosePledgeRequestData)_request.Data;

            Assert.That($"pledges/{_command.PledgeId}/close", Is.EqualTo(_request.PostUrl));
            Assert.That(_command.UserId, Is.EqualTo(data.UserId));
            Assert.That(_command.UserDisplayName, Is.EqualTo(data.UserDisplayName));
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }

        [Test]
        public async Task Pledge_Is_Closed_Unsuccessfull()
        {
            var error = _fixture.Create<InvalidOperationException>();
            var apiResponse = new ApiResponse<ClosePledgeRequest>(new ClosePledgeRequest(_command.PledgeId,
               new ClosePledgeRequest.ClosePledgeRequestData()), HttpStatusCode.NotFound, error.Message);

            _levyTransferMatchingService.Setup(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()))
               .Callback<ClosePledgeRequest>(r => _request = r)
               .ReturnsAsync(apiResponse);

            var response = await _handler.Handle(_command, CancellationToken.None);

            Assert.That(HttpStatusCode.NotFound, Is.EqualTo(response.StatusCode));
            Assert.That(error.Message, Is.EqualTo(response.ErrorContent));
        }
    }
}