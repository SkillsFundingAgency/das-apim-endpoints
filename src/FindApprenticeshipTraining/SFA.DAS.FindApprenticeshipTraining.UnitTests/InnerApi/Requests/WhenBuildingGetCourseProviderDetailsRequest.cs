using System;
using System.Globalization;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public sealed class WhenBuildingGetCourseProviderDetailsRequest
{
    [Test]
    public void Then_GetUrl_Includes_Encoded_Location()
    {
        var sut = new GetCourseProviderDetailsRequest("101", 10000003, "BT47 2DH", 123.456M, 54.321M, null);

        var result = sut.GetUrl;

        var expected = HttpUtility.UrlEncode("BT47 2DH");

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain($"location={expected}"));
            Assert.That(result, Does.StartWith("api/courses/101/providers/10000003/details?"));
        });
    }

    [Test]
    public void Then_GetUrl_Includes_Latitude_And_Longitude_When_Present()
    {
        var sut = new GetCourseProviderDetailsRequest("101", 10000003, string.Empty, 123.456M, 54.321M, null);

        var result = sut.GetUrl;

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.Contain("longitude=123.456"));
            Assert.That(result, Does.Contain("latitude=54.321"));
        });
    }

    [Test, AutoData]
    public void GetUrl_ShortlistUserIdProvided_IncludesShortlistUserId(string larsCode, long ukprn, Guid shortlistId)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, shortlistId);

        sut.GetUrl.Should().Contain($"shortlistUserId={shortlistId}");
    }

    [Test, AutoData]
    public void GetUrl_NoQueryParameters_ReturnsBaseUrlOnly(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, null);

        sut.GetUrl.Should().Be($"api/courses/{larsCode}/providers/{ukprn}/details");
    }

    [Test, AutoData]
    public void GetUrl_AllParametersProvided_BuildsValidUrl(string larsCode, long ukprn, string location, decimal longitude, decimal latitude, Guid shortlistId)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, location, longitude, latitude, shortlistId);

        var result = sut.GetUrl;

        result.Should().StartWith($"api/courses/{larsCode}/providers/{ukprn}/details?");
        result.Should().Contain($"location={HttpUtility.UrlEncode(location)}");
        result.Should().Contain($"latitude={latitude.ToString(CultureInfo.InvariantCulture)}");
        result.Should().Contain($"longitude={longitude.ToString(CultureInfo.InvariantCulture)}");
        result.Should().Contain($"shortlistUserId={shortlistId}");
    }

    [Test, AutoData]
    public void GetUrl_LocationIsWhitespace_ReturnsBaseUrlOnly(string larsCode, long ukprn)
    {
        string location = "  ";
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, location, null, null, null);

        sut.GetUrl.Should().Be($"api/courses/{larsCode}/providers/{ukprn}/details");
    }

    [Test, AutoData]
    public void Version_Default_ReturnsApiVersionTwo(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, null);

        sut.Version.Should().Be(ApiVersionNumber.Two);
    }
}
