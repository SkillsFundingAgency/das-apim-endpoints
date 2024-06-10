using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetLocations
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Locations_From_The_Api(
           GetLocationsListResponse getLocationsQueryResponse,
           GetLocationsQuery query,
           [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
           GetLocationsQueryHandler handler)
        {
            // Arrange
            mockLocationApiClient
                .Setup(client => client.Get<GetLocationsListResponse>(It.IsAny<GetLocationsQueryRequest>()))
                .ReturnsAsync(getLocationsQueryResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            // Assert
            actual.Locations.Should().BeEquivalentTo(getLocationsQueryResponse.Locations,
                options => options.ExcludingMissingMembers());
        }
    }
}
