using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

public record GetProviderCourseForecastsQueryResult(IEnumerable<ProviderCourseForecast> Forecasts);
