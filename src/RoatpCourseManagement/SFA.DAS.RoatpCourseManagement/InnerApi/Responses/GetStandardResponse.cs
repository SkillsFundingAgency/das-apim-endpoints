using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;

public class GetStandardResponse
{
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public string ApprovalBody { get; set; }
    public string Route { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public int Duration { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public CourseType CourseType { get; set; }

    public static implicit operator GetStandardResponse(GetStandardResponseFromCoursesApi source) =>
        new()
        {
            StandardUId = source.StandardUId,
            IfateReferenceNumber = source.IfateReferenceNumber,
            LarsCode = source.LarsCode.ToString(),
            Title = source.Title,
            Level = source.Level,
            ApprenticeshipType = source.ApprenticeshipType,
            ApprovalBody = source.ApprovalBody,
            Route = source.Route,
            IsRegulatedForProvider = source.IsRegulatedForProvider,
            Duration = source.Duration,
            DurationUnits = source.DurationUnits,
            CourseType = source.CourseType
        };
}

public class GetStandardResponseFromCoursesApi
{
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public int LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public string ApprovalBody { get; set; }
    public string Route { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public int Duration { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public CourseType CourseType { get; set; }
}

public class GetStandardResponseFromCourseManagementApi
{
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public string ApprovalBody { get; set; }
    public string Route { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public int Duration { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public CourseType CourseType { get; set; }
}