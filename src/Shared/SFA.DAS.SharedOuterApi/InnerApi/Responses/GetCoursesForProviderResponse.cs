using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses;

public class GetCoursesForProviderResponse
{
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? EffectiveTo { get; set; }
}