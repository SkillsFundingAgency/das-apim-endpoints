using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateApprentice;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
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

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Commands
{
    public class WhenHandlingCreateApprentice
    {
        private Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> _mockApiClient;
        private CreateApprenticeCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>>();
           
            _handler = new CreateApprenticeCommandHandler(_mockApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(CreateApprenticeCommand command,
            string errorContent)
        {
            var response = new ApiResponse<object>(null, HttpStatusCode.Created, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<object>(It.IsAny<PostCreateApprenticeRequest>()))
                .ReturnsAsync(response);

            await _handler.Handle(command, CancellationToken.None);

            _mockApiClient.Verify(c => c.PostWithResponseCode<object>(It.IsAny<IPostApiRequest>()));
        } 

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public void And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CreateApprenticeCommand command,
            string errorContent)
        {
            var response = new ApiResponse<object>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<object>(It.IsAny<PostCreateApprenticeRequest>()))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
