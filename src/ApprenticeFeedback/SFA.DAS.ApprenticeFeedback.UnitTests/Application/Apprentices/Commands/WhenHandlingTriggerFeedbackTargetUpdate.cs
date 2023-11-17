using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingTriggerFeedbackTargetUpdate
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockFeedbackApiClient;
        private Mock<IApprenticeshipDetailsService> _mockApprenticeshipDetailsService;
        private TriggerFeedbackTargetUpdateCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockFeedbackApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _mockApprenticeshipDetailsService = new Mock<IApprenticeshipDetailsService>();

            _handler = new TriggerFeedbackTargetUpdateCommandHandler(
                _mockFeedbackApiClient.Object, _mockApprenticeshipDetailsService.Object, Mock.Of<ILogger<TriggerFeedbackTargetUpdateCommandHandler>>());
        }

        [Test, MoqAutoData]
        public async Task Then_PostToUpdateFeedbackTargetIsSent(TriggerFeedbackTargetUpdateCommand command)
        {
            // Arrange
            _mockFeedbackApiClient
                .Setup(a => a.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.IsAny<UpdateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(new ApiResponse<ApprenticeFeedbackTarget>(new ApprenticeFeedbackTarget() { }, HttpStatusCode.OK, string.Empty));

            _mockApprenticeshipDetailsService
                .Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<long>()))
                .ReturnsAsync((new LearnerData(), new MyApprenticeshipData()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockFeedbackApiClient.Verify(c => c.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(It.Is<UpdateApprenticeFeedbackTargetRequest>(x => x.PostUrl == "api/apprenticefeedbacktarget/update"), true), Times.Once);
        }        
    }
}
