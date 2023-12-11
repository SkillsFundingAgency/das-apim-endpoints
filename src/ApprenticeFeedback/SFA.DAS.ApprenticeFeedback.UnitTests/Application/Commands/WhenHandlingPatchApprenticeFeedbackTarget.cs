using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.PatchApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Commands
{
    public class WhenHandlingPatchApprenticeFeedbackTarget
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockApiClient;
        private PatchApprenticeFeedbackTargetCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _handler = new PatchApprenticeFeedbackTargetCommandHandler(_mockApiClient.Object, Mock.Of<ILogger<PatchApprenticeFeedbackTargetCommandHandler>>());
        }

        [Test, MoqAutoData]
        public async Task Then_PatchRequestIsSent(PatchApprenticeFeedbackTargetCommand command, string errorContent)
        {
            var response = new ApiResponse<string>(null, HttpStatusCode.Created, errorContent);

            _mockApiClient.Setup(c => c.PatchWithResponseCode(It.IsAny<UpdateApprenticeFeedbackTargetStatusRequest>()))
                .ReturnsAsync(response);

            var result = await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(c => c.PatchWithResponseCode(It.IsAny<UpdateApprenticeFeedbackTargetStatusRequest>()));
            result.Should().BeOfType<PatchApprenticeFeedbackTargetResponse>();
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_FailSilentlyAndLogError(
            HttpStatusCode statusCode,
            PatchApprenticeFeedbackTargetCommand command,
            string errorContent)
        {
            var response = new ApiResponse<string>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PatchWithResponseCode(It.IsAny<UpdateApprenticeFeedbackTargetStatusRequest>()))
                .ReturnsAsync(response);

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeOfType<PatchApprenticeFeedbackTargetResponse>();
        }
    }
}
