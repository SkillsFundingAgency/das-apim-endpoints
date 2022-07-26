using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingTriggerFeedbackTargetUpdate
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockApiClient;
        private TriggerFeedbackTargetUpdateCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _handler = new TriggerFeedbackTargetUpdateCommandHandler(_mockApiClient.Object);
        }
        /*
        [Test, MoqAutoData]
        public async Task Then_PutRequestIsSent(TriggerFeedbackTargetUpdateCommand command,
            string errorContent)
        {
            var response = new ApiResponse<TriggerFeedbackTargetUpdateResponse>(null, HttpStatusCode.OK, errorContent);

            _mockApiClient.Setup(c => c.PutWithResponseCode<TriggerFeedbackTargetUpdateResponse>(It.IsAny<TriggerFeedbackTargetUpdateRequest>()))
                .ReturnsAsync(response);

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(c => c.PostWithResponseCode<CreateApprenticeFeedbackTargetResponse>(It.IsAny<IPutApiRequest>()));
        }
        */

        /*
        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public void And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CreateApprenticeFeedbackTargetCommand command,
            string errorContent)
        {
            var response = new ApiResponse<object>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<object>(It.IsAny<CreateApprenticeFeedbackTargetRequest>()))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<ApiResponseException>();
        }
        */
    }
}
