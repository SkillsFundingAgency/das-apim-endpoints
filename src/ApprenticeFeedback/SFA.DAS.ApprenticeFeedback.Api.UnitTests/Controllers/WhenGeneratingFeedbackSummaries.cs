using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.GenerateFeedbackSummaries;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenGeneratingFeedbackSummaries
    {
        private Mock<IMediator> _mockMediator;

        private DataLoadController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new DataLoadController(_mockMediator.Object, Mock.Of<ILogger<DataLoadController>>());
        }

        [Test, Theory]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnCreated()
        {
            var result = await _controller.GenerateFeedbackSummaries();
            result.Should().BeOfType<OkResult>();
        }

        [Test, Theory]
        public async Task And_CommandIsProcessedUnsuccessfully_Then_ReturnInternalServerError()
        {
            _mockMediator.Setup(s => s.Send(It.IsAny<GenerateFeedbackSummariesCommand>(), It.IsAny<CancellationToken>())).Throws<Exception>();
            var result = await _controller.GenerateFeedbackSummaries();
            result.Should().BeOfType<StatusCodeResult>().Which.StatusCode.Should().Be(500);
        }
    }
}
