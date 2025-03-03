using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.ProcessFeedbackTargetVariants;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Commands
{
    public class WhenHandlingFeedbackTargetVariant
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockApiClient;
        private ProcessFeedbackTargetVariantsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _handler = new ProcessFeedbackTargetVariantsCommandHandler(
                _mockApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(ProcessFeedbackTargetVariantsCommand command, string errorContent)
        {
            var response = new ApiResponse<NullResponse>(null, HttpStatusCode.OK, errorContent);
            var request = new ProcessFeedbackTargetVariantsRequest(new ProcessFeedbackTargetVariantsData(command.ClearStaging, command.MergeStaging, command.FeedbackTargetVariants));

            _mockApiClient.Setup(c => c.PostWithResponseCode<NullResponse>(It.IsAny<ProcessFeedbackTargetVariantsRequest>(), false))
                .ReturnsAsync(response);

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(c => c.PostWithResponseCode<NullResponse>(It.IsAny<IPostApiRequest>(), false), Times.Once);
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_(
            HttpStatusCode statusCode,
            ProcessFeedbackTargetVariantsCommand command,
            string errorContent)
        {
            var response = new ApiResponse<string>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<NullResponse>(It.IsAny<ProcessFeedbackTargetVariantsRequest>(), false))
                .ThrowsAsync(new ApiResponseException(statusCode, errorContent));

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
