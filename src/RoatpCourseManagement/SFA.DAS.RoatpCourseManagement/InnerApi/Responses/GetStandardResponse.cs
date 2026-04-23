using System;
using System.Linq;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

public class GetStandardResponse
{
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public LearningType ApprenticeshipType { get; set; }
    public string ApprovalBody { get; set; }
    public string Route { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public int Duration { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public CourseType CourseType { get; set; }
    public bool IsActiveAvailable { get; set; }

    public static implicit operator GetStandardResponse(GetStandardResponseFromCoursesApi source)
    {
        if (source == null) return null;

        var apprenticeshipFunding = source.ApprenticeshipFunding?.Count > 0 ?
            source.ApprenticeshipFunding.OrderByDescending(a => a.EffectiveFrom).First() : null;

        return new GetStandardResponse
        {
            StandardUId = source.StandardUId,
            IfateReferenceNumber = source.IfateReferenceNumber,
            LarsCode = source.LarsCode,
            Title = source.Title,
            Level = source.Level,
            ApprenticeshipType = source.LearningType,
            ApprovalBody = source.ApprovalBody,
            Route = source.Route,
            IsRegulatedForProvider = source.IsRegulatedForProvider,
            Duration = apprenticeshipFunding?.Duration ?? 0,
            DurationUnits = apprenticeshipFunding?.DurationUnits ?? default(DurationUnits),
            CourseType = source.CourseType,
            IsActiveAvailable = IsActiveAvailableStandard(source.CourseDates)
        };
    }

    private static bool IsActiveAvailableStandard(CourseDates courseDates)
    {
        var isActiveAvailable = (courseDates?.LastDateStarts == null) ||
                       (courseDates.LastDateStarts > DateTime.UtcNow
                       && courseDates.LastDateStarts != courseDates.EffectiveFrom);
        return isActiveAvailable;
    }
}