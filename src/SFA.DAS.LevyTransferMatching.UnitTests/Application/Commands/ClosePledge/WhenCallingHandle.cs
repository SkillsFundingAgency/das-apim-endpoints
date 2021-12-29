using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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

            Assert.AreEqual($"pledges/{_command.PledgeId}/close", _request.PostUrl);
            Assert.AreEqual(_command.UserId, data.UserId);
            Assert.AreEqual(_command.UserDisplayName, data.UserDisplayName);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
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

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual(error.Message, response.ErrorContent);
        }
    }
}