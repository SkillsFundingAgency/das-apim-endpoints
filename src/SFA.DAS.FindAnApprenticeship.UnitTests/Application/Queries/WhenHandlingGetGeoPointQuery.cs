using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetGeoPoint;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
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