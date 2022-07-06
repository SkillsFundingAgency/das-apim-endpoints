using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Locations.Queries.GetLocations;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Locations.Queries
{
    public class WhenGettingLocationsBySearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Location_Api(
            GetLocationsQuery query,
            GetLocationsListResponse apiResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetLocationsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c=>c.GetUrl.Contains(query.SearchTerm))))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Locations.Should().BeEquivalentTo(apiResponse.Locations);
        }
    }
}