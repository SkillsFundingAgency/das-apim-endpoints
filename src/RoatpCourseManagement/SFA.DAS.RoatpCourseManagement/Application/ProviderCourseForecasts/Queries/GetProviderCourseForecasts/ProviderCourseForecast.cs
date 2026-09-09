using System;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

public class ProviderCourseForecast
{
    public string TimePeriod { get; set; }
    public int Quarter { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? EstimatedLearners { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
