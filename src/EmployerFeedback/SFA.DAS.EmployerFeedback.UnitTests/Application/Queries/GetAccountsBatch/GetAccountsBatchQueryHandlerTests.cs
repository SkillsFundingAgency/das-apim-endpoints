using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAccountsBatch;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetAccountsBatch
{
    [TestFixture]
    public class GetAccountsBatchQueryHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _apiClientMock;
        private GetAccountsBatchQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _handler = new GetAccountsBatchQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsAccountIds_WhenApiResponseIsSuccess()
        {
            var batchSize = 10;
            var accountIds = new List<long> { 1, 2, 3, 4, 5 };
            var responseBody = new GetAccountsBatchResponse { AccountIds = accountIds };
            var apiResponse = new ApiResponse<GetAccountsBatchResponse>(responseBody, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetAccountsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.Is<GetAccountsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetAccountsBatchQuery(batchSize), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccountIds, Is.EqualTo(accountIds));
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.IsAny<GetAccountsBatchRequest>()), Times.Once);
        }

        [Test]
        public void Handle_ThrowsException_WhenApiResponseIsNotSuccess()
        {
            var batchSize = 10;
            var apiResponse = new ApiResponse<GetAccountsBatchResponse>(null, HttpStatusCode.InternalServerError, "Error");
            var expectedRequest = new GetAccountsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.Is<GetAccountsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            Assert.ThrowsAsync<SharedOuterApi.Exceptions.ApiResponseException>(async () =>
                await _handler.Handle(new GetAccountsBatchQuery(batchSize), CancellationToken.None));
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.IsAny<GetAccountsBatchRequest>()), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsNullAccountIds_WhenApiResponseBodyIsNullButStatusIsSuccess()
        {
            var batchSize = 10;
            var apiResponse = new ApiResponse<GetAccountsBatchResponse>(null, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetAccountsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.Is<GetAccountsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetAccountsBatchQuery(batchSize), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccountIds, Is.Null);
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.IsAny<GetAccountsBatchRequest>()), Times.Once);
        }

        [Test]
        public async Task Handle_ReturnsEmptyAccountIds_WhenApiResponseBodyHasEmptyAccountIds()
        {
            var batchSize = 10;
            var responseBody = new GetAccountsBatchResponse { AccountIds = new List<long>() };
            var apiResponse = new ApiResponse<GetAccountsBatchResponse>(responseBody, HttpStatusCode.OK, string.Empty);
            var expectedRequest = new GetAccountsBatchRequest(batchSize);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.Is<GetAccountsBatchRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await _handler.Handle(new GetAccountsBatchQuery(batchSize), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AccountIds, Is.Not.Null);
            Assert.That(result.AccountIds, Is.Empty);
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetAccountsBatchResponse>(It.IsAny<GetAccountsBatchRequest>()), Times.Once);
        }
    }
}