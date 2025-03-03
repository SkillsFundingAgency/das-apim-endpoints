using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSettings;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    [TestFixture]
    public class GetSettingsQueryHandlerTests
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _mockApiClient;
        private GetSettingsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _handler = new GetSettingsQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Handle_Should_Return_Settings_From_Api()
        {
            // Arrange
            var query = new GetSettingsQuery();
            var apiResponse = new GetSettingsResponse
            {
                ExpiryAfterMonths = 3,
                ProviderRemovedAfterRequestedMonths = 12
            };

            _mockApiClient
                .Setup(client => client.Get<GetSettingsResponse>(It.IsAny<GetSettingsRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ExpiryAfterMonths.Should().Be(apiResponse.ExpiryAfterMonths);
            result.RemovedAfterRequestedMonths.Should().Be(apiResponse.ProviderRemovedAfterRequestedMonths);

            _mockApiClient.Verify(client => client.Get<GetSettingsResponse>(It.IsAny<GetSettingsRequest>()), Times.Once);
        }

        [Test]
        public async Task Handle_Should_Throw_Exception_If_Api_Call_Fails()
        {
            // Arrange
            var query = new GetSettingsQuery();

            _mockApiClient
                .Setup(client => client.Get<GetSettingsResponse>(It.IsAny<GetSettingsRequest>()))
                .ThrowsAsync(new HttpRequestException());

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<HttpRequestException>();

            _mockApiClient.Verify(client => client.Get<GetSettingsResponse>(It.IsAny<GetSettingsRequest>()), Times.Once);
        }
    }
}
