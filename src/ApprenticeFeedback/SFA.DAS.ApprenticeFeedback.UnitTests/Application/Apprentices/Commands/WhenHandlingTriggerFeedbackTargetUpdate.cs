using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingTriggerFeedbackTargetUpdate
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private Mock<IAssessorsApiClient<AssessorsApiConfiguration>> _mockAssessorApiClient;
        private TriggerFeedbackTargetUpdateCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _mockAssessorApiClient = new Mock<IAssessorsApiClient<AssessorsApiConfiguration>>();

            _handler = new TriggerFeedbackTargetUpdateCommandHandler(
                _mockFeedbackApiClient.Object, _mockAssessorApiClient.Object, Mock.Of<ILogger<TriggerFeedbackTargetUpdateCommandHandler>>());
        }

        [Test, MoqAutoData]
        public async Task Then_PostToUpdateFeedbackTargetIsSent(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockAssessorApiClient.Setup(a => a.GetWithResponseCode<GetApprenticeLearnerResponse>(It.IsAny<GetApprenticeLearnerRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeLearnerResponse>(new GetApprenticeLearnerResponse() { }, HttpStatusCode.OK, string.Empty));

            _mockFeedbackApiClient.Setup(a => a.PostWithResponseCode<ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<ApprenticeFeedbackTarget>(It.Is<UpdateApprenticeFeedbackTargetRequest>(x => x.PostUrl == "api/apprenticefeedbacktarget/update"), true), Times.Once);
        }
    }
}
