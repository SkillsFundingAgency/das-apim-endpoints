using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetLocations
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Locations_From_The_Api(
           GetLocationsQuery query,
           [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
           GetLocationsQueryHandler handler)
        {
            // Arrange
            var locationsListResponse = new ApiResponse<GetLocationsListResponse>(new GetLocationsListResponse
            {
                Locations = new[]
                {
                    new GetLocationsListItem { LocationName = "TestLocation1", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } },
                    new GetLocationsListItem { LocationName = "TestLocation2", LocalAuthorityName = "Authority", Location = new GetLocationsListItem.Coordinates { GeoPoint = [1.0, 2.0] } }
                }
            }, HttpStatusCode.OK, string.Empty);

            mockLocationApiClient
                .Setup(client => client.GetWithResponseCode<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(locationsListResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.Locations.Should().BeEquivalentTo(locationsListResponse.Body.Locations,
                options => options.ExcludingMissingMembers());
        }
    }
}
