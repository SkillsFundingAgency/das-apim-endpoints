using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Application.Review;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Aodp.Application.Tests.Commands.Application
{
    [TestFixture]
    public class SaveReviewerCommandHandlerTests
    {
        private static readonly Guid ApplicationId = Guid.NewGuid();
        private const string ReviewerFieldName = "Reviewer1";
        private const string ReviewerValue = "New Reviewer";
        private const string SentByName = "Test User";
        private const string SentByEmail = "user@test.com";
        private const string UserType = "Qfau";
        private const string ExceptionMessage = "api exception";

        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClient = null!;
        private Mock<ILogger<SaveReviewerCommandHandler>> _logger = null!;
        private SaveReviewerCommandHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _apiClient = new Mock<IAodpApiClient<AodpApiConfiguration>>();
            _logger = new Mock<ILogger<SaveReviewerCommandHandler>>();

            _handler = new SaveReviewerCommandHandler(_apiClient.Object, _logger.Object);
        }

        [Test]
        public async Task Handle_ValidResponse_CallsApiClientPut_AndReturnsSuccess()
        {
            var innerResponse = new SaveReviewerCommandResponse
            {
                DuplicateReviewerError = false
            };

            var apiResponse = new ApiResponse<SaveReviewerCommandResponse>(
                innerResponse,
                HttpStatusCode.OK,
                string.Empty);

            _apiClient
                .Setup(c => c.PutWithResponseCode<SaveReviewerCommandResponse>(
                    It.IsAny<SaveReviewerApiRequest>()))
                .ReturnsAsync(apiResponse);

            var request = new SaveReviewerCommand
            {
                ApplicationId = ApplicationId,
                ReviewerFieldName = ReviewerFieldName,
                ReviewerValue = ReviewerValue,
                SentByName = SentByName,
                SentByEmail = SentByEmail,
                UserType = UserType
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.True);
                Assert.That(result.ErrorMessage, Is.Null.Or.Empty);
                Assert.That(result.Value, Is.Not.Null);

                _apiClient.Verify(c =>
                    c.PutWithResponseCode<SaveReviewerCommandResponse>(
                        It.Is<SaveReviewerApiRequest>(r =>
                            r.ApplicationId == ApplicationId &&
                            r.Data == request)),
                    Times.Once);

                Assert.That(result.Value, Is.SameAs(innerResponse));
            });
        }

        [Test]
        public async Task Handle_ApiClientThrows_ReturnsErrorResponse()
        {
            _apiClient
                .Setup(c => c.PutWithResponseCode<SaveReviewerCommandResponse>(
                    It.IsAny<SaveReviewerApiRequest>()))
                .ThrowsAsync(new InvalidOperationException(ExceptionMessage));

            var request = new SaveReviewerCommand
            {
                ApplicationId = ApplicationId,
                ReviewerFieldName = ReviewerFieldName,
                ReviewerValue = ReviewerValue,
                SentByName = SentByName,
                SentByEmail = SentByEmail,
                UserType = UserType
            };

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Success, Is.False);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.ErrorMessage, Is.EqualTo(ExceptionMessage));

                _apiClient.Verify(c =>
                    c.PutWithResponseCode<SaveReviewerCommandResponse>(
                        It.IsAny<SaveReviewerApiRequest>()),
                    Times.Once);
            });
        }
    }
}
