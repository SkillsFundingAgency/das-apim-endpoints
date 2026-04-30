using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class GetProviderCourseForecastsRequestTests
{
    [Test, AutoData]
    public void GetUrl_ReturnsExpected(int ukprn, string larsCode)
    {
        var sut = new GetProviderCourseForecastsRequest(ukprn, larsCode);

        sut.GetUrl.Should().Be($"providers/{ukprn}/courses/{larsCode}/forecasts");
    }
}
