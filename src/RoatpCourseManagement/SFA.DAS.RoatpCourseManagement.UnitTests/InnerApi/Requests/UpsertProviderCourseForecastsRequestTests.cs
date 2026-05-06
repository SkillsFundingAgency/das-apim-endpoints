using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

public class UpsertProviderCourseForecastsRequestTests
{
    [Test, AutoData]
    public void PostUrl_Should_Return_Correct_Url(
        int ukprn,
        string larsCode,
        IEnumerable<UpsertProviderCourseForecastModel> forecasts)
    {
        // Arrange
        var request = new UpsertProviderCourseForecastsRequest(ukprn, larsCode, forecasts);
        // Act
        request.PostUrl.Should().Be($"providers/{ukprn}/courses/{larsCode}/forecasts");
    }
}
