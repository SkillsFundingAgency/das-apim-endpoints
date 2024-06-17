using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetLocation
    {
        private Mock<ILocationApiClient<LocationApiConfiguration>> _mockApiClient;
        private GetLocationQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockApiClient = new Mock<ILocationApiClient<LocationApiConfiguration>>();
            _handler = new GetLocationQueryHandler(_mockApiClient.Object);
        }

        [Test]
        public async Task Then_Returns_Location_If_ExactMatch_Found()
        {
            // Arrange
            var query = new GetLocationQuery { ExactMatch = "TestLocation, Authority" };
            var locationsListResponse = new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            };

            _mockApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(locationsListResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().NotBeNull();
            result.Location.DisplayName.Should().Be("TestLocation, Authority");
        }

        [Test]
        public async Task Then_Returns_Empty_Result_If_No_ExactMatch_Found()
        {
            // Arrange
            var query = new GetLocationQuery { ExactMatch = "TestLocation, Authority" };
            var locationsListResponse = new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation, OtherAuthoity", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            };

            _mockApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(locationsListResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().BeNull();
        }

        [Test]
        public async Task Then_Returns_Empty_Result_If_Query_Is_Less_Than_Three_Characters()
        {
            // Arrange
            var query = new GetLocationQuery { ExactMatch = "Te" };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().BeNull();
            _mockApiClient.Verify(client => client.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()), Times.Never);
        }

        [Test]
        public async Task Then_Returns_Empty_Result_If_Query_Is_Empty()
        {
            // Arrange
            var query = new GetLocationQuery { ExactMatch = string.Empty };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().BeNull();
            _mockApiClient.Verify(client => client.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()), Times.Never);
        }
    }
}