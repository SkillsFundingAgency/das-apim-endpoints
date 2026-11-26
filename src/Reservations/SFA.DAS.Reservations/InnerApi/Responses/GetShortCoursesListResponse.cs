using System;
using System.Collections.Generic;

namespace SFA.DAS.Reservations.InnerApi.Responses;

public class GetShortCoursesListResponse
{
    public int Total { get; set; }
    
    public int TotalFiltered { get; set; }
    
    public IEnumerable<ShortCourseListItem> Courses { get; set; }
}

public class ShortCourseListItem
{
    public string CourseUId { get; set; }
    
    public string ReferenceNumber { get; set; }
    
    public string LarsCode { get; set; }
    
    public string Title { get; set; }
    
    public string LevelCode { get; set; }
    
    public ShortCourseDates CourseDates { get; set; }
    
    public string CourseType { get; set; }
    
    public string LearningType { get; set; }
}

public class ShortCourseDates
{
    public object LastDateStarts { get; set; }
    
    public DateTime? EffectiveTo { get; set; }
    
    public DateTime? EffectiveFrom { get; set; }
}