using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingIndexLocationQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_And_Location_Returned(
            IndexLocationQuery query,
            LocationItem locationServiceResponse,
            [Frozen] Mock<ILocationLookupService> locationService,
            IndexLocationQueryHandler handler)
        {
            locationService.Setup(x => x.GetLocationInformation(query.LocationSearchTerm, 0, 0, false))
                .ReturnsAsync(locationServiceResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.LocationItem.Should().BeEquivalentTo(locationServiceResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Match_Null_Returned(
            IndexLocationQuery query,
            [Frozen] Mock<ILocationLookupService> locationService,
            IndexLocationQueryHandler handler)
        {
            locationService.Setup(x => x.GetLocationInformation(query.LocationSearchTerm, 0, 0, false))
                .ReturnsAsync((LocationItem)null);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.LocationItem.Should().BeNull();
        }
    }
}