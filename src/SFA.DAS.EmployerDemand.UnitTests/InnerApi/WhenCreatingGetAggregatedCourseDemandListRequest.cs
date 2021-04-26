using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetAggregatedCourseDemandListRequest
    {
        [Test, AutoData]
        public void Then_Sets_Url_Correctly(
            int ukprn,
            int courseId,
            double lat,
            double lon,
            int radius,
            List<string> sectors)
        {
            var request = new GetAggregatedCourseDemandListRequest(ukprn, courseId, lat, lon, radius, sectors);
            request.GetUrl.Should().Be($"api/demand/aggregated/providers/{request.Ukprn}?courseId={request.CourseId}&lat={request.Lat}&lon={request.Lon}&radius={request.Radius}&sectors=" + string.Join("&sectors=", sectors.Select(HttpUtility.UrlEncode)));
            request.CourseId.Should().Be(courseId);
            request.Lat.Should().Be(lat);
            request.Lon.Should().Be(lon);
            request.Radius.Should().Be(radius);
            request.Sectors.Should().BeEquivalentTo(sectors);
        }
    }
}