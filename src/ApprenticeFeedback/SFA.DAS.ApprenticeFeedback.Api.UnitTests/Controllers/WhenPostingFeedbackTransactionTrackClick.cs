using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TrackEmailTransactionClick;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPostingFeedbackTransactionTrackClick
    {
        private Mock<IMediator> _mockMediator;

        private FeedbackTransactionController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new FeedbackTransactionController(
                _mockMediator.Object, 
                Mock.Of<ILogger<FeedbackTransactionController>>());
        }

        [Test, MoqAutoData]
        public async Task And_TrackClick_ShouldReturnOk_WhenClickIsSuccessfullyTracked()
        {
            // Arrange
            var feedbackTransactionId = 123;
            var feedbackTransactionClick = new FeedbackTransactionClick
            {
                ApprenticeFeedbackTargetId = Guid.NewGuid()
            };
            
            var expectedResponse = new TrackEmailTransactionClickResponse
            {
                ClickStatus = ClickStatus.Valid
            };

            _mockMediator.Setup(m => m.Send(It.IsAny<TrackEmailTransactionClickCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.TrackClick(feedbackTransactionId, feedbackTransactionClick) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.Value.Should().BeOfType(typeof(TrackEmailTransactionClickResult));
        }
    }
}
