using System;
using System.Web;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

[TestFixture]
public sealed class WhenBuildingGetCourseProviderDetailsRequest
{
    [Test, MoqAutoData]
    public void GetUrl_LocationProvided_ReturnsUrlWithEncodedLocation(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, "BT47 2DH", 123.456M, 54.321M, null);

        var result = sut.GetUrl;

        var expected = HttpUtility.UrlEncode("BT47 2DH");

        result.Should().StartWith($"api/courses/{larsCode}/providers/{ukprn}/details?");
        result.Should().Contain($"location={expected}");
    }

    [Test, MoqAutoData]
    public void GetUrl_LatitudeAndLongitudeProvided_ReturnsUrlWithLatLong(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, 123.456M, 54.321M, null);

        var result = sut.GetUrl;

        result.Should().Contain("longitude=123.456");
        result.Should().Contain("latitude=54.321");
    }

    [Test, MoqAutoData]
    public void GetUrl_WithShortlistUserId_IncludesShortlistUserId(string larsCode, long ukprn, Guid shortlistId)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, shortlistId);

        sut.GetUrl.Should().Contain($"shortlistUserId={shortlistId}");
    }

    [Test, MoqAutoData]
    public void GetUrl_NoQueryParameters_ReturnsBaseUrlOnly(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, null);

        sut.GetUrl.Should().Be($"api/courses/{larsCode}/providers/{ukprn}/details");
    }

    [Test, MoqAutoData]
    public void GetUrl_AllParameters_BuildsValidUrl(string larsCode, long ukprn, Guid shortlistId)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, "SW1 111", -1.234M, 53.123M, shortlistId);

        var result = sut.GetUrl;

        result.Should().StartWith($"api/courses/{larsCode}/providers/{ukprn}/details?");
        result.Should().Contain($"location={HttpUtility.UrlEncode("SW1 111")}");
        result.Should().Contain("latitude=53.123");
        result.Should().Contain("longitude=-1.234");
        result.Should().Contain($"shortlistUserId={shortlistId}");
    }

    [Test, MoqAutoData]
    public void Version_Always_ReturnsApiVersionTwo(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, string.Empty, null, null, null);

        sut.Version.Should().Be(ApiVersionNumber.Two);
    }

    [Test, MoqAutoData]
    public void GetUrl_WhitespaceOnlyLocation_Ignored_ReturnsBaseUrl(string larsCode, long ukprn)
    {
        var sut = new GetCourseProviderDetailsRequest(larsCode, ukprn, "   ", null, null, null);

        sut.GetUrl.Should().Be($"api/courses/{larsCode}/providers/{ukprn}/details");
    }
}
