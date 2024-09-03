using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.AcknowledgeEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingCreateProviderResponseEmployerRequests
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private AcknowledgeEmployerRequestsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();

            _handler = new AcknowledgeEmployerRequestsCommandHandler(_mockApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(AcknowledgeEmployerRequestsCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<object>(new object(), HttpStatusCode.Created, errorContent);
        
            _mockApiClient.Setup(c => c.PostWithResponseCode<CreateEmployerResponseEmployerRequestData, object>(It.IsAny<CreateProviderResponseEmployerRequestRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PostWithResponseCode<CreateEmployerResponseEmployerRequestData, object>(It.IsAny<CreateProviderResponseEmployerRequestRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestSentIsCommand(AcknowledgeEmployerRequestsCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<object>(new object(), HttpStatusCode.Created, errorContent);
            IPostApiRequest<CreateEmployerResponseEmployerRequestData> submittedRequest = null;

            _mockApiClient.Setup(c => c.PostWithResponseCode<CreateEmployerResponseEmployerRequestData, object>(It.IsAny<CreateProviderResponseEmployerRequestRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<CreateEmployerResponseEmployerRequestData>, bool>((x, y) => submittedRequest = x)
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            submittedRequest.Data.Should().BeEquivalentTo(new
            {
                command.Ukprn,
            });
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            AcknowledgeEmployerRequestsCommand command,
            string errorContent)
        {
            var response = new ApiResponse<object>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<CreateEmployerResponseEmployerRequestData, object>(It.IsAny<CreateProviderResponseEmployerRequestRequest>(), false))
            .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
