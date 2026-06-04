using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

public record GetProviderCourseForecastsQueryResult(string LarsCode, string CourseName, int CourseLevel, IEnumerable<ProviderCourseForecast> Forecasts);
