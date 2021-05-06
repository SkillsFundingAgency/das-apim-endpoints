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
            List<string> routes)
        {
            var request = new GetAggregatedCourseDemandListRequest(ukprn, courseId, lat, lon, radius, routes);
            request.GetUrl.Should().Be($"api/demand/aggregated/providers/{request.Ukprn}?courseId={request.CourseId}&lat={request.Lat}&lon={request.Lon}&radius={request.Radius}&routes=" + string.Join("&routes=", routes.Select(HttpUtility.UrlEncode)));
            request.CourseId.Should().Be(courseId);
            request.Lat.Should().Be(lat);
            request.Lon.Should().Be(lon);
            request.Radius.Should().Be(radius);
            request.Routes.Should().BeEquivalentTo(routes);
        }

        [Test, AutoData]
        public void Then_Sets_Url_If_Routes_Is_Empty(
            int ukprn,
            int courseId,
            double lat,
            double lon,
            int radius)
        {
            var request = new GetAggregatedCourseDemandListRequest(ukprn, courseId, lat, lon, radius, new List<string>());
            request.GetUrl.Should().Be($"api/demand/aggregated/providers/{request.Ukprn}?courseId={request.CourseId}&lat={request.Lat}&lon={request.Lon}&radius={request.Radius}&routes=");
            request.CourseId.Should().Be(courseId);
            request.Lat.Should().Be(lat);
            request.Lon.Should().Be(lon);
            request.Radius.Should().Be(radius);
            request.Routes.Should().BeEquivalentTo(new List<string>());
        }

        [Test, AutoData]
        public void Then_Sets_Url_If_Routes_Is_Null(
            int ukprn,
            int courseId,
            double lat,
            double lon,
            int radius)
        {
            var request = new GetAggregatedCourseDemandListRequest(ukprn, courseId, lat, lon, radius);
            request.GetUrl.Should().Be($"api/demand/aggregated/providers/{request.Ukprn}?courseId={request.CourseId}&lat={request.Lat}&lon={request.Lon}&radius={request.Radius}&routes=");
            request.CourseId.Should().Be(courseId);
            request.Lat.Should().Be(lat);
            request.Lon.Should().Be(lon);
            request.Radius.Should().Be(radius);
            request.Routes.Should().BeNull();
        }
    }
}