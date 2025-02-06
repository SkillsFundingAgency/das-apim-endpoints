using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;
public class WhenBuildingGetProvidersByCourseIdRequest
{
    private const int CourseId = 1;
    private ProviderOrderBy OrderBy = ProviderOrderBy.Distance;
    private const string BaseUrl = "api/courses/1/providers?OrderBy=Distance";

    [Test, AutoData]
    public void Then_The_Base_Url_Is_Correctly_Constructed(int courseId, ProviderOrderBy orderBy)
    {
        var actual = new GetProvidersByCourseIdRequest(courseId, orderBy);
        actual.GetUrl.Should().Be($"api/courses/{courseId}/providers?OrderBy={orderBy}");
    }


    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Distance(decimal? distance)
    {
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, distance);
        actual.GetUrl.Should().Be($"{BaseUrl}&distance={distance}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Latitude(decimal? latitude)
    {
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, latitude);
        actual.GetUrl.Should().Be($"{BaseUrl}&latitude={latitude}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Longitude(decimal? longitude)
    {
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, longitude);
        actual.GetUrl.Should().Be($"{BaseUrl}&longitude={longitude}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_One_DeliveryMode(DeliveryMode deliveryMode)
    {
        var deliveryModes = new List<DeliveryMode?> { deliveryMode };
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, deliveryModes);
        actual.GetUrl.Should().Be($"{BaseUrl}&DeliveryModes={deliveryMode}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Multiple_DeliveryModes(List<DeliveryMode?> deliveryModes)
    {
        var addedUrl = string.Empty;
        if (deliveryModes is { Count: > 0 })
        {
            addedUrl += "&DeliveryModes=" + string.Join("&deliveryModes=", deliveryModes);
        }

        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, deliveryModes);
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_One_EmployerProviderRate(ProviderRating providerRating)
    {
        var providerRatings = new List<ProviderRating?> { providerRating };
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, providerRatings);
        actual.GetUrl.Should().Be($"{BaseUrl}&employerProviderRatings={providerRating}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Multiple_EmployerProviderRates(List<ProviderRating?> providerRatings)
    {
        var addedUrl = string.Empty;
        if (providerRatings is { Count: > 0 })
        {
            addedUrl += "&employerProviderRatings=" + string.Join("&employerProviderRatings=", providerRatings);
        }

        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, providerRatings);
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_One_ApprenticeProviderRate(ProviderRating providerRating)
    {
        var providerRatings = new List<ProviderRating?> { providerRating };
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, null, providerRatings);
        actual.GetUrl.Should().Be($"{BaseUrl}&apprenticeProviderRatings={providerRating}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Multiple_ApprenticeProviderRates(List<ProviderRating?> providerRatings)
    {
        var addedUrl = string.Empty;
        if (providerRatings is { Count: > 0 })
        {
            addedUrl += "&apprenticeProviderRatings=" + string.Join("&apprenticeProviderRatings=", providerRatings);
        }

        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, null, providerRatings);
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_One_Qar(QarRating qarRating)
    {
        var qarRatings = new List<QarRating?> { qarRating };
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, null, null, qarRatings);
        actual.GetUrl.Should().Be($"{BaseUrl}&qar={qarRating}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Multiple_Qars(List<QarRating?> qarRatings)
    {
        var addedUrl = string.Empty;
        if (qarRatings is { Count: > 0 })
        {
            addedUrl += "&qar=" + string.Join("&qar=", qarRatings);
        }

        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, null, null, qarRatings);
        actual.GetUrl.Should().Be($"{BaseUrl}{addedUrl}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_Page(int? page)
    {
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, null, null, null, page);
        actual.GetUrl.Should().Be($"{BaseUrl}&page={page}");
    }

    [Test, AutoData]
    public void Then_Url_Is_Constructed_With_PageSize(int? pageSize)
    {
        var actual = new GetProvidersByCourseIdRequest(CourseId, OrderBy, null, null, null, null, null, null, null, null, pageSize);
        actual.GetUrl.Should().Be($"{BaseUrl}&pageSize={pageSize}");
    }
}
