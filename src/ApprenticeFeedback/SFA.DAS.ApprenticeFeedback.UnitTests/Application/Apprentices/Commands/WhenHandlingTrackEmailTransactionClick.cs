using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TrackEmailTransactionClick;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.ApprenticeFeedback.Models.Enums;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingTrackEmailTransactionClick
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private TrackEmailTransactionClickCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();

            _handler = new TrackEmailTransactionClickCommandHandler(
                _mockFeedbackApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostToTrackEmailTransactionClick(TrackEmailTransactionClickCommand command)
        {
            // Arrange
            var expectedResponse = new TrackEmailTransactionClickResponse { ClickStatus = ClickStatus.Valid};

            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<TrackEmailTransactionClickResponse>(It.IsAny<TrackEmailTransactionClickRequest>(), true))
                .ReturnsAsync(new ApiResponse<TrackEmailTransactionClickResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<TrackEmailTransactionClickResponse>(
                It.Is<TrackEmailTransactionClickRequest>(x =>
                    ArePropertiesMatching(command, (TrackEmailTransactionClickData)x.Data)
                ), true), Times.Once);
        }

        private bool ArePropertiesMatching(TrackEmailTransactionClickCommand command, TrackEmailTransactionClickData data)
        {
            return data.LinkName == command.LinkName &&
                   data.FeedbackTransactionId == command.FeedbackTransactionId &&
                   data.ApprenticeFeedbackTargetId == command.ApprenticeFeedbackTargetId &&
                   data.LinkUrl == command.LinkUrl &&
                   data.ClickedOn == command.ClickedOn;
        }

    }
}