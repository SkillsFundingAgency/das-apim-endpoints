using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

public class GetStandardResponseFromCoursesApi
{
    public List<ApprenticeshipFunding> ApprenticeshipFunding { get; set; } = new();
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public LearningType LearningType { get; set; }
    public string ApprovalBody { get; set; }
    public string Route { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public CourseType CourseType { get; set; }
    public CourseDates CourseDates { get; set; }
}
public class ApprenticeshipFunding
{
    public DateTime EffectiveFrom { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public int Duration { get; set; }
}

public class CourseDates
{
    public DateTime? LastDateStarts { get; set; }
    public DateTime EffectiveTo { get; set; }
    public DateTime? EffectiveFrom { get; set; }
}
