using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.GenerateFeedbackSummaries;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.GenerateFeedbackSummaries
{
    [TestFixture]
    public class GenerateFeedbackSummariesCommandHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _apiClientMock;
        private GenerateFeedbackSummariesCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _handler = new GenerateFeedbackSummariesCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_CallsApiClient_AndSucceeds_WhenApiReturnsSuccess()
        {
            var expectedRequest = new GenerateFeedbackSummariesRequest();
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty);
            _apiClientMock
                .Setup(x => x.PostWithResponseCode<object>(
                    It.Is<GenerateFeedbackSummariesRequest>(r =>
                        r.PostUrl == expectedRequest.PostUrl && r.Data == expectedRequest.Data),
                    false))
                .ReturnsAsync(apiResponse);

            Assert.DoesNotThrowAsync(async () =>
                await _handler.Handle(new GenerateFeedbackSummariesCommand(), CancellationToken.None));
        }

        [Test]
        public void Handle_Throws_WhenApiReturnsNonSuccess()
        {
            var expectedRequest = new GenerateFeedbackSummariesRequest();
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "Error");
            _apiClientMock
                .Setup(x => x.PostWithResponseCode<object>(
                    It.Is<GenerateFeedbackSummariesRequest>(r =>
                        r.PostUrl == expectedRequest.PostUrl && r.Data == expectedRequest.Data),
                    false))
                .ReturnsAsync(apiResponse);

            Assert.ThrowsAsync<SFA.DAS.SharedOuterApi.Exceptions.ApiResponseException>(async () =>
                await _handler.Handle(new GenerateFeedbackSummariesCommand(), CancellationToken.None));
        }
    }
}
