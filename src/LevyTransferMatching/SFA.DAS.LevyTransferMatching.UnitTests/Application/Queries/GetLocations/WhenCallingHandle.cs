using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetLocations
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Api_Called_And_Location_Display_Names_Returned(
            GetLocationsQuery getLocationsQuery,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            GetLocationsQueryHandler getLocationsQueryHandler)
        {
            mockLocationApiClient
                .Setup(x => x.Get<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(y => y.GetUrl.Contains(getLocationsQuery.SearchTerm))))
                .ReturnsAsync(new GetLocationsListResponse { Locations = new List<GetLocationsListItem>
                {
                    new GetLocationsListItem { LocationName = "Manchester", LocalAuthorityName = "Greater Manchester" },
                    new GetLocationsListItem { LocationName = "Warwick", LocalAuthorityName = "Warwickshire" },
                    new GetLocationsListItem { LocationName = "Newquay", LocalAuthorityName = "Cornwall" }
                }});

            var result = await getLocationsQueryHandler.Handle(getLocationsQuery, CancellationToken.None);

            foreach (var location in result.Locations)
            {
                Assert.That(location.DisplayName, Is.Not.Null);
                Assert.That(location.DisplayName, Is.Not.EqualTo(""));
            }
        }
    }
}
