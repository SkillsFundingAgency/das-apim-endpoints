using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingAddApprentice
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockApiClient;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _mockCommitmentsApiClient;
        private CreateApprenticeCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _handler = new CreateApprenticeCommandHandler(_mockCommitmentsApiClient.Object, _mockApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(CreateApprenticeCommand command,
            string errorContent)
        {
            var response = new ApiResponse<PostAddApprenticeRequest>(null, HttpStatusCode.Created, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<PostAddApprenticeRequest>(It.IsAny<PostAddApprenticeRequest>()))
                .ReturnsAsync(response);

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(c => c.PostWithResponseCode<PostAddApprenticeRequest>(It.IsAny<IPostApiRequest>()));
        } 

        [Test, MoqAutoData]
        public void And_ApiDoesNotReturnCreated_Then_ThrowException(
            CreateApprenticeCommand command,
            string errorContent)
        {
            var response = new ApiResponse<PostAddApprenticeRequest>(null, HttpStatusCode.BadRequest, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<PostAddApprenticeRequest>(It.IsAny<PostAddApprenticeRequest>()))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<HttpRequestContentException>();
        }
    }
}
