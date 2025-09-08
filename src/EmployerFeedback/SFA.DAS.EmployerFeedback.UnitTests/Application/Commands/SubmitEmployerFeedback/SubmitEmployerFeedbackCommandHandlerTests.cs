using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Commands.SubmitEmployerFeedback;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.EmployerFeedback.Models;
using System.Collections.Generic;
using SubmitEmployerFeedbackRequest = SFA.DAS.EmployerFeedback.InnerApi.Requests.SubmitEmployerFeedbackRequest;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Commands.SubmitEmployerFeedback
{
    [TestFixture]
    public class SubmitEmployerFeedbackCommandHandlerTests
    {
        private Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _apiClientMock;
        private SubmitEmployerFeedbackCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>>();
            _handler = new SubmitEmployerFeedbackCommandHandler(_apiClientMock.Object);
        }

        [Test]
        public Task Handle_CallsApiClient_AndSucceeds_WhenApiReturnsSuccess()
        {
            var command = new SubmitEmployerFeedbackCommand
            {
                UserRef = System.Guid.NewGuid(),
                Ukprn = 12345,
                AccountId = 67890,
                ProviderRating = OverallRating.Good,
                FeedbackSource = 2,
                ProviderAttributes = new List<ProviderAttributeDto> { new ProviderAttributeDto { AttributeId = 1, AttributeValue = 5 } }
            };
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty);
            _apiClientMock
                .Setup(x => x.PostWithResponseCode<SubmitEmployerFeedbackRequestData, object>(
                    It.IsAny<SubmitEmployerFeedbackRequest>(), false))
                .ReturnsAsync(apiResponse);

            Assert.DoesNotThrowAsync(async () =>
                await _handler.Handle(command, CancellationToken.None));
            return Task.CompletedTask;
        }

        [Test]
        public void Handle_Throws_WhenApiReturnsNonSuccess()
        {
            var command = new SubmitEmployerFeedbackCommand
            {
                UserRef = System.Guid.NewGuid(),
                Ukprn = 12345,
                AccountId = 67890,
                ProviderRating = OverallRating.Good,
                FeedbackSource = 2,
                ProviderAttributes = new List<ProviderAttributeDto> { new ProviderAttributeDto { AttributeId = 1, AttributeValue = 5 } }
            };
            var apiResponse = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "Error");
            _apiClientMock
                .Setup(x => x.PostWithResponseCode<SubmitEmployerFeedbackRequestData, object>(
                    It.IsAny<SubmitEmployerFeedbackRequest>(), false))
                .ReturnsAsync(apiResponse);

            Assert.ThrowsAsync<SharedOuterApi.Exceptions.ApiResponseException>(async () =>
                await _handler.Handle(command, CancellationToken.None));
        }
    }
}
