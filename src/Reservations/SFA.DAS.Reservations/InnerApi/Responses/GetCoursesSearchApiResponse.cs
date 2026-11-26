using System;
using System.Collections.Generic;

namespace SFA.DAS.Reservations.InnerApi.Responses;

public class GetCoursesSearchApiResponse
{
    public IEnumerable<CourseSearchItemDto> Courses { get; set; }
    public int Total { get; set; }
    public int TotalFiltered { get; set; }
}

public class CourseSearchItemDto
{
    public string StandardUId { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public CourseDatesDto CourseDates { get; set; }
    public string LearningType { get; set; }
    public string CourseType { get; set; }
}

public class CourseDatesDto
{
    public DateTime? LastDateStarts { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public DateTime EffectiveFrom { get; set; }
}
