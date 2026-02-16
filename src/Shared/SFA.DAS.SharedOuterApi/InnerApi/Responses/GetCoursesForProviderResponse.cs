using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;

public class GetCoursesForProviderResponse
{
    public long Ukprn { get; set; }

    public string Status { get; set; }

    public string Type { get; set; }

    public List<CourseTypes> CourseTypes { get; set; }
}

public class CourseTypes
{
    public string CourseType { get; set; }

    public List<Course> Courses { get; set; }
}

public class Course
{
    public string Larscode { get; set; }

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }
}