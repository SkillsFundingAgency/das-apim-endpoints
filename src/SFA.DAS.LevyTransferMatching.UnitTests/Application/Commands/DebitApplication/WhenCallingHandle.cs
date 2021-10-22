using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DebitApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private DebitApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private readonly Fixture _fixture = new Fixture();

        private DebitApplicationCommand _command;
        private DebitApplicationRequest _request;

        [SetUp]
        public void Setup()
        {
            _command = _fixture.Create<DebitApplicationCommand>();

            var apiResponse = new ApiResponse<DebitApplicationRequest>(new DebitApplicationRequest(_command.ApplicationId, new DebitApplicationRequest.DebitApplicationRequestData()), HttpStatusCode.OK, string.Empty);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.DebitApplication(It.IsAny<DebitApplicationRequest>()))
                .Callback<DebitApplicationRequest>(r => _request = r)
                .ReturnsAsync(apiResponse);

            _handler = new DebitApplicationCommandHandler(_levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Application_Is_Debited()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var debit = (DebitApplicationRequest.DebitApplicationRequestData)_request.Data;

            Assert.AreEqual($"applications/{_command.ApplicationId}/debit", _request.PostUrl);
            Assert.AreEqual(_command.Amount, debit.Amount);
            Assert.AreEqual(_command.NumberOfApprentices, debit.NumberOfApprentices);
        }
    }
}
