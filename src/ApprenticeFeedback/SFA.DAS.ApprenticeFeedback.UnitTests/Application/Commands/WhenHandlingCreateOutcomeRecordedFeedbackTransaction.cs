using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateOutcomeRecordedFeedbackTransaction;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Commands
{
    public class WhenHandlingCreateOutcomeRecordedFeedbackTransaction
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private CreateOutcomeRecordedFeedbackTransactionCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _handler = new CreateOutcomeRecordedFeedbackTransactionCommandHandler(_mockFeedbackApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostsToInnerApi_AndReturnsSuccess(CreateOutcomeRecordedFeedbackTransactionCommand command)
        {
            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<object>(It.IsAny<CreateOutcomeRecordedFeedbackTransactionRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.NotFound.Should().BeFalse();
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<object>(
                It.Is<CreateOutcomeRecordedFeedbackTransactionRequest>(x =>
                    x.PostUrl == "api/feedbacktransaction/outcome-recorded"
                    && ((CreateOutcomeRecordedFeedbackTransactionData)x.Data).ApprenticeshipId == command.ApprenticeshipId),
                true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_InnerApiReturnsNotFound_Then_ReturnsNotFound(CreateOutcomeRecordedFeedbackTransactionCommand command)
        {
            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<object>(It.IsAny<CreateOutcomeRecordedFeedbackTransactionRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.NotFound, string.Empty));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.NotFound.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task And_InnerApiReturnsServerError_Then_Throws(CreateOutcomeRecordedFeedbackTransactionCommand command)
        {
            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<object>(It.IsAny<CreateOutcomeRecordedFeedbackTransactionRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "error"));

            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>();
        }
    }
}
