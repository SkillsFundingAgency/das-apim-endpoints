using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
            var query = new GetLocationQuery { ExactSearchTerm = "TestLocation, Authority" };
            var locationsListResponse = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            }, HttpStatusCode.OK, string.Empty);

            _mockApiClient
                .Setup(client => client.GetWithResponseCode<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
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
            var query = new GetLocationQuery { ExactSearchTerm = "TestLocation, Authority" };

            var locationsListResponse = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation, OtherAuthority", LocalAuthorityName = "OtherAuthority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            }, HttpStatusCode.OK, string.Empty);

            _mockApiClient
                .Setup(client => client.GetWithResponseCode<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
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
            var query = new GetLocationQuery { ExactSearchTerm = "Te" };

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
            var query = new GetLocationQuery { ExactSearchTerm = string.Empty };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().BeNull();
            _mockApiClient.Verify(client => client.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()), Times.Never);
        }

        [Test]
        public async Task Then_Returns_Empty_Result_If_No_Locations_Found_In_Api()
        {
            // Arrange
            var query = new GetLocationQuery { ExactSearchTerm = "TestLocation" };
            var locationsListResponse = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse { Locations = null }, HttpStatusCode.OK, string.Empty);
            
            _mockApiClient
                .Setup(client => client.GetWithResponseCode<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(locationsListResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().BeNull();
        }

        [Test]
        public async Task Then_Searches_Are_Performed_For_Substrings_Of_Increasing_Length()
        {
            // Arrange
            var query = new GetLocationQuery { ExactSearchTerm = "TestLocation4, Authority" };
            var emptyResponse = new GetLocationsListResponse { Locations = new GetLocationsListItem[0] };
            var locationsListResponseOne = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation1", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } },
                    new GetLocationsListItem { LocationName = "TestLocation2", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            }, HttpStatusCode.OK, string.Empty);

            var locationsListResponseTwo = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation2", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } },
                    new GetLocationsListItem { LocationName = "TestLocation3", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            }, HttpStatusCode.OK, string.Empty);

            var locationsListResponseThree = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation3", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } },
                    new GetLocationsListItem { LocationName = "TestLocation4", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            }, HttpStatusCode.OK, string.Empty);

            _mockApiClient
                .SetupSequence(client => client.GetWithResponseCode<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(locationsListResponseOne) 
                .ReturnsAsync(locationsListResponseTwo) 
                .ReturnsAsync(locationsListResponseThree); 

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Location.Should().NotBeNull();
            result.Location.DisplayName.Should().Be("TestLocation4, Authority");

            // Verify that the API client was called with substrings of increasing length
            _mockApiClient.Verify(client => client.GetWithResponseCode<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(r => r.GetUrl == $"api/search?query={HttpUtility.UrlEncode("Tes")}")), Times.Once);
            _mockApiClient.Verify(client => client.GetWithResponseCode<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(r => r.GetUrl == $"api/search?query={HttpUtility.UrlEncode("Test")}")), Times.Once);
            _mockApiClient.Verify(client => client.GetWithResponseCode<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(r => r.GetUrl == $"api/search?query={HttpUtility.UrlEncode("TestL")}")), Times.Once);
        }
    }
}
