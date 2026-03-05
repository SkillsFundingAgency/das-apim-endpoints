using System;
using System.Globalization;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Common;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

[TestFixture]
public sealed class WhenBuildingGetCourseProviderDetailsRequest
{
    [Test, AutoData]
    public void GetUrl_LocationProvided_ReturnsUrlWithEncodedLocation(string larsCode, long ukprn, decimal? longitude, decimal? latitude)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, "BT47 2DH", longitude, latitude, null);

        var result = sut.GetUrl;

        var expected = HttpUtility.UrlEncode("BT47 2DH");

        result.Should().StartWith($"api/courses/{larsCode}/providers/{ukprn}/details?");
        result.Should().Contain($"location={expected}");
    }

    [Test, AutoData]
    public void GetUrl_LatitudeAndLongitudeProvided_ReturnsUrlWithLatLong(string larsCode, long ukprn, decimal? longitude, decimal? latitude)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, longitude, latitude, null);

        var result = sut.GetUrl;

        result.Should().Contain($"longitude={longitude}");
        result.Should().Contain($"latitude={latitude}");
    }

    [Test, AutoData]
    public void GetUrl_WithShortlistUserId_IncludesShortlistUserId(string larsCode, long ukprn, Guid shortlistId)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, shortlistId);

        sut.GetUrl.Should().Contain($"shortlistUserId={shortlistId}");
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
