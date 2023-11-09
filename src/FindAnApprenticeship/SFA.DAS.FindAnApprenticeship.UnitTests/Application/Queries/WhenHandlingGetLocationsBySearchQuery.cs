using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;
using System.Threading;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using FluentAssertions;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingGetLocationsBySearchQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Locations_From_Location_Api(
            GetLocationsBySearchQuery query,
            GetLocationsListResponse apiResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetLocationsBySearchQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(
                    It.Is<GetLocationsQueryRequest>(c => c.GetUrl.Contains(query.SearchTerm))))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Locations.Should().BeEquivalentTo(apiResponse.Locations);
        }
    }
}