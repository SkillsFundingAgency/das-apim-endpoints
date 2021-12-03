using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetGeocode;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAddresses
{
    public class WhenHandlingGetGeoPointQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Calls_Location_Service_To_Get_GeoPoint(
            GetGeoPointQuery query,
            GetGeoPointResponse apiResponse,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            GetGeoPointQueryHandler handler)
        {
            var result = await handler.Handle(query, CancellationToken.None);
            mockLocationLookupService.Verify(p => p.GetLocationInformation(query.Postcode, default(double), default(double), false), Times.Once);
        }
    }
}
