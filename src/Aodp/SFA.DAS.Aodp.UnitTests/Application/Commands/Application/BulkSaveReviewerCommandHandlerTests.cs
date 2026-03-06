using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.InnerApi.Application.Review;
using SFA.DAS.AODP.Application.Commands.Application.Review;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Application.Review
{
    [TestFixture]
    public class BulkSaveReviewerCommandHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private BulkSaveReviewerCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new BulkSaveReviewerCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            var command = _fixture.Create<BulkSaveReviewerCommand>();

            var apiResponse = new BulkSaveReviewerCommandResponse
            {
                RequestedCount = 3,
                UpdatedCount = 3,
                ErrorCount = 0
            };

            _apiClientMock
                .Setup(x => x.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    It.IsAny<BulkSaveReviewerApiRequest>()))
                .ReturnsAsync(new ApiResponse<BulkSaveReviewerCommandResponse>(
                    apiResponse, HttpStatusCode.OK, ""));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value.RequestedCount, Is.EqualTo(apiResponse.RequestedCount));
                Assert.That(result.Value.UpdatedCount, Is.EqualTo(apiResponse.UpdatedCount));
                Assert.That(result.Value.ErrorCount, Is.EqualTo(apiResponse.ErrorCount));
            });

            _apiClientMock.Verify(x =>
                x.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    It.IsAny<BulkSaveReviewerApiRequest>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsError_WhenApiCallThrows()
        {
            // Arrange
            var command = _fixture.Create<BulkSaveReviewerCommand>();
            var exceptionMessage = "API failed";

            _apiClientMock
                .Setup(x => x.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    It.IsAny<BulkSaveReviewerApiRequest>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
            });

            _apiClientMock.Verify(x =>
                x.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    It.IsAny<BulkSaveReviewerApiRequest>()),
                Times.Once);
        }

        [Test]
        public async Task Handle_CallsApiClientWithCorrectRequest()
        {
            // Arrange
            var command = _fixture.Create<BulkSaveReviewerCommand>();

            _apiClientMock
                .Setup(x => x.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    It.IsAny<BulkSaveReviewerApiRequest>()))
                .ReturnsAsync(new ApiResponse<BulkSaveReviewerCommandResponse>(
                    new BulkSaveReviewerCommandResponse(), HttpStatusCode.OK, ""));

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x =>
                x.PutWithResponseCode<BulkSaveReviewerCommandResponse>(
                    It.Is<BulkSaveReviewerApiRequest>(req =>
                        req.Data != null &&
                        ((BulkSaveReviewerCommand)req.Data).SentByEmail == command.SentByEmail &&
                        ((BulkSaveReviewerCommand)req.Data).SentByName == command.SentByName &&
                        ((BulkSaveReviewerCommand)req.Data).Reviewer1 == command.Reviewer1 &&
                        ((BulkSaveReviewerCommand)req.Data).Reviewer2 == command.Reviewer2 &&
                        ((BulkSaveReviewerCommand)req.Data).ApplicationIds.SequenceEqual(command.ApplicationIds)
                    )),
                Times.Once);


        }
    }
}
