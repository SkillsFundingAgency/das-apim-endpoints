using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetRegions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.UnitTests.Queries
{
    public class WhenHandlingGetRegionsQuery
    {
        private Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>> _apiClientMock;
        private GetRegionsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _apiClientMock = new Mock<IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration>>();
            _handler = new GetRegionsQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Then_Returns_All_Regions_When_Api_Call_Is_Successful()
        {
            // Arrange
            var query = new GetRegionsQuery();
            var regions = new List<Region>
            {
                new Region { Id = 1, SubregionName = "London" },
                new Region { Id = 2, SubregionName = "Manchester" }
            };
            var apiResponse = new ApiResponse<List<Region>>(regions, System.Net.HttpStatusCode.OK, string.Empty);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<List<Region>>(It.IsAny<GetRegionsRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Regions.Should().BeEquivalentTo(regions);
        }

        [Test]
        public void Then_Throws_Exception_When_Api_Call_Is_Unsuccessful()
        {
            // Arrange
            var query = new GetRegionsQuery();
            var apiResponse = new ApiResponse<List<Region>>(null, System.Net.HttpStatusCode.InternalServerError, string.Empty);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<List<Region>>(It.IsAny<GetRegionsRequest>()))
                .ReturnsAsync(apiResponse);

            // Act
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<HttpRequestException>();
        }
    }
}
