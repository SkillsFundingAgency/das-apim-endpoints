using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetClosestRegion;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenHandlingGetClosestRegionQuery
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _apiClientMock;
        private GetClosestRegionQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _handler = new GetClosestRegionQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Then_Returns_Closest_Region_When_Api_Call_Is_Successful()
        {
            // Arrange
            var query = new GetClosestRegionQuery { Latitude = 51.5074, Longitude = -0.1278 }; // Example coordinates
            var region = new Region { Id = 1, SubregionName = "London" };
            var apiResponse = new ApiResponse<Region>(region, System.Net.HttpStatusCode.OK, string.Empty);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<Region>(It.IsAny<GetClosestRegionRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Region.Should().BeEquivalentTo(region);
        }

        [Test]
        public void Then_Throws_Exception_When_Api_Call_Is_Unsuccessful()
        {
            // Arrange
            var query = new GetClosestRegionQuery { Latitude = 51.5074, Longitude = -0.1278 }; // Example coordinates
            var apiResponse = new ApiResponse<Region>(null, System.Net.HttpStatusCode.InternalServerError, string.Empty);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<Region>(It.IsAny<GetClosestRegionRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
