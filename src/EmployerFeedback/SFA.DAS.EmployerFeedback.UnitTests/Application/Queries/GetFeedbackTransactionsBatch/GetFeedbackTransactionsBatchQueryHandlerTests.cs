using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetFeedbackTransactionsBatch;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetFeedbackTransactionsBatch
{
    [TestFixture]
    public class GetFeedbackTransactionsBatchQueryHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _apiClientMock;
        private GetFeedbackTransactionsBatchQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _handler = new GetFeedbackTransactionsBatchQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsFeedbackTransactions_WhenApiResponseIsSuccess()
        {
            var batchSize = 10;
            var feedbackTransactions = new List<long> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var responseBody = new GetFeedbackTransactionsBatchResponse { FeedbackTransactions = feedbackTransactions };
            var apiResponse = new ApiResponse<GetFeedbackTransactionsBatchResponse>(responseBody, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetFeedbackTransactionsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(It.Is<GetFeedbackTransactionsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetFeedbackTransactionsBatchQuery(batchSize), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FeedbackTransactions, Is.EqualTo(feedbackTransactions));
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(It.IsAny<GetFeedbackTransactionsBatchRequest>()), Times.Once);
        }

        [Test]
        public void Handle_ThrowsException_WhenApiResponseIsNotSuccess()
        {
            var batchSize = 10;
            var apiResponse = new ApiResponse<GetFeedbackTransactionsBatchResponse>(null, HttpStatusCode.InternalServerError, "Error");
            var expectedRequest = new GetFeedbackTransactionsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(It.Is<GetFeedbackTransactionsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            Assert.ThrowsAsync<SharedOuterApi.Exceptions.ApiResponseException>(async () =>
                await _handler.Handle(new GetFeedbackTransactionsBatchQuery(batchSize), CancellationToken.None));
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(It.IsAny<GetFeedbackTransactionsBatchRequest>()), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsEmptyFeedbackTransactions_WhenApiResponseBodyHasEmptyFeedbackTransactions()
        {
            var batchSize = 10;
            var responseBody = new GetFeedbackTransactionsBatchResponse { FeedbackTransactions = new List<long>() };
            var apiResponse = new ApiResponse<GetFeedbackTransactionsBatchResponse>(responseBody, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetFeedbackTransactionsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(It.Is<GetFeedbackTransactionsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetFeedbackTransactionsBatchQuery(batchSize), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.FeedbackTransactions, Is.Not.Null);
            Assert.That(result.FeedbackTransactions, Is.Empty);
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetFeedbackTransactionsBatchResponse>(It.IsAny<GetFeedbackTransactionsBatchRequest>()), Times.Once);
        }
    }
}