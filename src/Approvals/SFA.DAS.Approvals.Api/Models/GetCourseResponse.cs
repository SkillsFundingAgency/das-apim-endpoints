using System;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Types;

namespace SFA.DAS.Approvals.Api.Models;

public class GetCourseResponse
{
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public LearningType LearningType { get; set; }
    public int MaxFunding { get; set; }
    public DateTime? EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }

    public static implicit operator GetCourseResponse(GetCoursesListItem source)
    {
        return new GetCourseResponse
        {
            LarsCode = source.LarsCode.ToString(),
            Title = source.Title,
            Level = source.Level,
            LearningType = source.LearningType,
            MaxFunding = source.MaxFunding,
            EffectiveFrom = source.CourseDates?.EffectiveFrom,
            EffectiveTo = source.CourseDates?.EffectiveTo
        };
    }
}