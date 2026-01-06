using System;
using System.Web;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

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

    [Test]
    public void Then_GetUrl_Includes_ShortlistUserId_When_Present()
    {
        var shortlistId = Guid.NewGuid();

        var sut = new GetCourseProviderDetailsRequest("111", 10000003, string.Empty, null, null, shortlistId);

        Assert.That(sut.GetUrl, Does.Contain($"shortlistUserId={shortlistId}"));
    }

    [Test]
    public void Then_GetUrl_Without_QueryParameters_Returns_BaseUrl_Only()
    {
        var sut = new GetCourseProviderDetailsRequest("999", 10000003, string.Empty, null, null, null);

        Assert.That(sut.GetUrl, Is.EqualTo("api/courses/999/providers/10000003/details"));
    }

    [Test]
    public void Then_GetUrl_With_All_Parameters_Builds_Valid_Url()
    {
        var shortlistId = Guid.NewGuid();

        var sut = new GetCourseProviderDetailsRequest("123", 10000003, "SW1 111", -1.234M, 53.123M, shortlistId);

        var result = sut.GetUrl;

        Assert.Multiple(() =>
        {
            Assert.That(result, Does.StartWith("api/courses/123/providers/10000003/details?"));
            Assert.That(result, Does.Contain($"location={HttpUtility.UrlEncode("SW1 111")}"));
            Assert.That(result, Does.Contain("latitude=53.123"));
            Assert.That(result, Does.Contain("longitude=-1.234"));
            Assert.That(result, Does.Contain($"shortlistUserId={shortlistId}"));
        });
    }
}
