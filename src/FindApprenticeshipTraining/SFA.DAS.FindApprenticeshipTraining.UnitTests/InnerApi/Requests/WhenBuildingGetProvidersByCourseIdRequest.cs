using System.Collections.Generic;
using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;
public class WhenBuildingGetProvidersByCourseIdRequest
{
    private const string CourseId = "1";
    private ProviderOrderBy OrderBy = ProviderOrderBy.Distance;
    private const string BaseUrl = "api/courses/1/providers?OrderBy=Distance";

    [Test, AutoData]
    public void GetUrl_WithOrderBy_ConstructsBaseUrl(string courseId, ProviderOrderBy orderBy)
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = courseId, OrderBy = orderBy };
        actual.GetUrl.Should().Be($"api/courses/{courseId}/providers?OrderBy={orderBy}");
    }


    [Test, AutoData]
    public void GetUrl_WithDistance_AppendsDistance(decimal? distance)
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Distance = distance };
        actual.GetUrl.Should().Be($"{BaseUrl}&distance={distance}");
    }

    [Test, AutoData]
    public void GetUrl_WithLatitude_AppendsLatitude(decimal? latitude)
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Latitude = latitude };
        actual.GetUrl.Should().Be($"{BaseUrl}&latitude={latitude}");
    }

    [Test, AutoData]
    public void GetUrl_WithLongitude_AppendsLongitude(decimal? longitude)
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Longitude = longitude };
        actual.GetUrl.Should().Be($"{BaseUrl}&longitude={longitude}");
    }

    [Test, AutoData]
    public void GetUrl_WithSingleDeliveryMode_AppendsDeliveryMode(DeliveryMode deliveryMode)
    {
        var deliveryModes = new List<DeliveryMode?> { deliveryMode };
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, DeliveryModes = deliveryModes };
        actual.GetUrl.Should().Be($"{BaseUrl}&DeliveryModes={deliveryMode}");
    }

    [Test, AutoData]
    public void GetUrl_WithMultipleDeliveryModes_AppendsAllDeliveryModes(List<DeliveryMode?> deliveryModes)
    {
        var addedUrl = string.Empty;
        if (deliveryModes is { Count: > 0 })
        {
            addedUrl += "&DeliveryModes=" + string.Join("&deliveryModes=", deliveryModes);
        }

        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, DeliveryModes = deliveryModes };
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void GetUrl_WithSingleEmployerRating_AppendsEmployerRating(ProviderRating providerRating)
    {
        var providerRatings = new List<ProviderRating?> { providerRating };
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, EmployerProviderRatings = providerRatings };
        actual.GetUrl.Should().Be($"{BaseUrl}&employerProviderRatings={providerRating}");
    }

    [Test, AutoData]
    public void GetUrl_WithMultipleEmployerRatings_AppendsAllEmployerRatings(List<ProviderRating?> providerRatings)
    {
        var addedUrl = string.Empty;
        if (providerRatings is { Count: > 0 })
        {
            addedUrl += "&employerProviderRatings=" + string.Join("&employerProviderRatings=", providerRatings);
        }

        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, EmployerProviderRatings = providerRatings };
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void GetUrl_WithSingleApprenticeRating_AppendsApprenticeRating(ProviderRating providerRating)
    {
        var providerRatings = new List<ProviderRating?> { providerRating };
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, ApprenticeProviderRatings = providerRatings };
        actual.GetUrl.Should().Be($"{BaseUrl}&apprenticeProviderRatings={providerRating}");
    }

    [Test, AutoData]
    public void GetUrl_WithMultipleApprenticeRatings_AppendsAllApprenticeRatings(List<ProviderRating?> providerRatings)
    {
        var addedUrl = string.Empty;
        if (providerRatings is { Count: > 0 })
        {
            addedUrl += "&apprenticeProviderRatings=" + string.Join("&apprenticeProviderRatings=", providerRatings);
        }

        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, ApprenticeProviderRatings = providerRatings };
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void GetUrl_WithSingleQarRating_AppendsQar(QarRating qarRating)
    {
        var qarRatings = new List<QarRating?> { qarRating };
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Qar = qarRatings };
        actual.GetUrl.Should().Be($"{BaseUrl}&qar={qarRating}");
    }

    [Test, AutoData]
    public void GetUrl_WithMultipleQarRatings_AppendsAllQar(List<QarRating?> qarRatings)
    {
        var addedUrl = string.Empty;
        if (qarRatings is { Count: > 0 })
        {
            addedUrl += "&qar=" + string.Join("&qar=", qarRatings);
        }

        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Qar = qarRatings };
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void GetUrl_WithPage_AppendsPage(int? page)
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Page = page };
        actual.GetUrl.Should().Be($"{BaseUrl}&page={page}");
    }

    [Test, AutoData]
    public void GetUrl_WithPageSize_AppendsPageSize(int? pageSize)
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, PageSize = pageSize };
        actual.GetUrl.Should().Be($"{BaseUrl}&pageSize={pageSize}");
    }

    [Test]
    public void Version_WhenRequested_ReturnsTwo()
    {
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy };
        actual.Version.Should().Be("2.0");
    }

    [Test]
    public void GetUrl_WithLocation_AppendsLocation()
    {
        var location = "London";
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, Location = location };
        actual.GetUrl.Should().Be($"{BaseUrl}&location={location}");
    }

    [Test]
    public void GetUrl_WithUserId_AppendsUserId()
    {
        var userId = Guid.NewGuid();
        var actual = new GetProvidersByCourseIdRequest() { CourseId = CourseId, OrderBy = OrderBy, UserId = userId };
        actual.GetUrl.Should().Be($"{BaseUrl}&userId={userId}");
    }
}
