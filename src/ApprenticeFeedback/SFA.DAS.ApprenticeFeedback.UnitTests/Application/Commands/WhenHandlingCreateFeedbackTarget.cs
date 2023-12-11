using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Commands
{
    public class WhenHandlingCreateApprenticeFeedbackTarget
    {
        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(
            CreateApprenticeFeedbackTargetCommand command,
            string errorContent,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApiClient,
            CreateApprenticeFeedbackTargetCommandHandler handler)
        {
            var response = new ApiResponse<CreateApprenticeFeedbackTargetResponse>(null, HttpStatusCode.Created, errorContent);

            mockApiClient.Setup(c => c.PostWithResponseCode<CreateApprenticeFeedbackTargetResponse>(It.IsAny<CreateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(response);

            var actual = await handler.Handle(command, CancellationToken.None);

            actual.Should().BeEquivalentTo(response.Body);
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public void And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CreateApprenticeFeedbackTargetCommand command,
            string errorContent,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApiClient,
            CreateApprenticeFeedbackTargetCommandHandler handler)
        {
            var response = new ApiResponse<CreateApprenticeFeedbackTargetResponse>(null, statusCode, errorContent);

            mockApiClient.Setup(c => c.PostWithResponseCode<CreateApprenticeFeedbackTargetResponse>(It.IsAny<CreateApprenticeFeedbackTargetRequest>(), true))
                .ReturnsAsync(response);

            Assert.ThrowsAsync<ApiResponseException>(() => handler.Handle(command, CancellationToken.None));

        }
    }
}
