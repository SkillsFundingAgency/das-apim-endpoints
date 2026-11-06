using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateFeedbackTransaction;
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
    public class WhenHandlingCreateFeedbackTransaction
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private CreateFeedbackTransactionCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _handler = new CreateFeedbackTransactionCommandHandler(_mockFeedbackApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostsToInnerApi_AndReturnsSuccess(CreateFeedbackTransactionCommand command)
        {
            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<object>(It.IsAny<CreateFeedbackTransactionRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.NoContent, string.Empty));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.NotFound.Should().BeFalse();
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<object>(
                It.Is<CreateFeedbackTransactionRequest>(x =>
                    x.PostUrl == "api/feedbacktransaction"
                    && ((CreateFeedbackTransactionData)x.Data).ApprenticeshipId == command.ApprenticeshipId
                    && ((CreateFeedbackTransactionData)x.Data).TemplateName == command.TemplateName),
                true), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_InnerApiReturnsNotFound_Then_ReturnsNotFound(CreateFeedbackTransactionCommand command)
        {
            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<object>(It.IsAny<CreateFeedbackTransactionRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.NotFound, string.Empty));

            var result = await _handler.Handle(command, CancellationToken.None);

            result.NotFound.Should().BeTrue();
        }

        [Test, MoqAutoData]
        public async Task And_InnerApiReturnsServerError_Then_Throws(CreateFeedbackTransactionCommand command)
        {
            _mockFeedbackApiClient
                .Setup(x => x.PostWithResponseCode<object>(It.IsAny<CreateFeedbackTransactionRequest>(), true))
                .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "error"));

            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<Exception>();
        }
    }
}
