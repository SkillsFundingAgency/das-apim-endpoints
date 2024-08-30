using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Commands.ExpireEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Commands
{
    public class WhenHandlingExpireEmployerRequests
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private ExpireEmployerRequestsCommandHandler _handler;

        [SetUp]
        public void Arrange()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();

            _handler = new ExpireEmployerRequestsCommandHandler(_mockApiClient.Object);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestIsSent(ExpireEmployerRequestsCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<ExpireEmployerRequestsResponse>(new ExpireEmployerRequestsResponse(), HttpStatusCode.OK, errorContent);
        
            _mockApiClient.Setup(c => c.PostWithResponseCode<ExpireEmployerRequestsData, ExpireEmployerRequestsResponse>(It.IsAny<ExpireEmployerRequestsRequest>(), It.IsAny<bool>()))
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockApiClient.Verify(c => c.PostWithResponseCode<ExpireEmployerRequestsData, ExpireEmployerRequestsResponse>(It.IsAny<ExpireEmployerRequestsRequest>(), It.IsAny<bool>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PostRequestSentIsCommand(ExpireEmployerRequestsCommand command, string errorContent)
        {
            // Arrange
            var response = new ApiResponse<ExpireEmployerRequestsResponse>(new ExpireEmployerRequestsResponse(), HttpStatusCode.Created, errorContent);
            IPostApiRequest<ExpireEmployerRequestsData> submittedRequest = null;

            _mockApiClient.Setup(c => c.PostWithResponseCode<ExpireEmployerRequestsData, ExpireEmployerRequestsResponse>(It.IsAny<ExpireEmployerRequestsRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<ExpireEmployerRequestsData>, bool>((x, y) => submittedRequest = x)
                .ReturnsAsync(response);

            // Act
            await _handler.Handle(command, CancellationToken.None);

        }

        [Test]
        [MoqInlineAutoData(HttpStatusCode.BadRequest)]
        [MoqInlineAutoData(HttpStatusCode.InternalServerError)]
        [MoqInlineAutoData(HttpStatusCode.NotFound)]
        public async Task And_ApiDoesNotReturnSuccess_Then_ThrowApiResponseException(
            HttpStatusCode statusCode,
            ExpireEmployerRequestsCommand command,
            string errorContent)
        {
            var response = new ApiResponse<ExpireEmployerRequestsResponse>(null, statusCode, errorContent);

            _mockApiClient.Setup(c => c.PostWithResponseCode<ExpireEmployerRequestsData, ExpireEmployerRequestsResponse>(It.IsAny<ExpireEmployerRequestsRequest>(), true))
                .ReturnsAsync(response);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>();
        }
    }
}
