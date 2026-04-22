namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;

public record UpsertProviderCourseForecastModel(string TimePeriod, int Quarter, int? EstimatedLearners);
