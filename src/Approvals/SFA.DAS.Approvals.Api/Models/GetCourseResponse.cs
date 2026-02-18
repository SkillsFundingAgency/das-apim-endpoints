using System;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.Models;

public class GetCourseResponse
{
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public string LearningType { get; set; }
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
            LearningType = source.ApprenticeshipType,
            //MaxFunding = source.MaxFunding,
            //EffectiveFrom = source.StandardDates?.EffectiveFrom,
            //EffectiveTo = source.StandardDates?.EffectiveTo
        };
    }
}