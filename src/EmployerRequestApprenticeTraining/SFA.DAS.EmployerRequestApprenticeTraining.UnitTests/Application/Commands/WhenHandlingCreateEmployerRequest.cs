using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingCreateEmployerRequest
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private CreateEmployerRequestCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();

            _handler = new CreateEmployerRequestCommandHandler(_mockApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(CreateEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<CreateEmployerRequestResponse>(new CreateEmployerRequestResponse(), HttpStatusCode.Created, errorContent);
        
            _mockApiClient.Setup(c => c.PostWithResponseCode<CreateEmployerRequestData, CreateEmployerRequestResponse>(It.IsAny<CreateEmployerRequestRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PostWithResponseCode<CreateEmployerRequestData, CreateEmployerRequestResponse>(It.IsAny<CreateEmployerRequestRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestSentIsCommand(CreateEmployerRequestCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<CreateEmployerRequestResponse>(new CreateEmployerRequestResponse(), HttpStatusCode.Created, errorContent);
            IPostApiRequest<CreateEmployerRequestData> submittedRequest = null;

            _mockApiClient.Setup(c => c.PostWithResponseCode<CreateEmployerRequestData, CreateEmployerRequestResponse>(It.IsAny<CreateEmployerRequestRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<CreateEmployerRequestData>, bool>((x, y) => submittedRequest = x)
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            submittedRequest.Data.Should().BeEquivalentTo(new
            {
                command.RequestType
            });
        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            CreateEmployerRequestCommand command,
            string errorContent)
        {
            var response = new ApiResponse<CreateEmployerRequestResponse>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<CreateEmployerRequestData, CreateEmployerRequestResponse>(It.IsAny<CreateEmployerRequestRequest>(), true))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
