using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;

public class GetStandardInformationQueryResult
{
    public string StandardUId { get; set; }
    public string IfateReferenceNumber { get; set; }
    public string LarsCode { get; set; }
    public string Title { get; set; }
    public int Level { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public string ApprovalBody { get; set; }
    public string Route { get; set; }
    public int Duration { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public CourseType CourseType { get; set; }

    public static implicit operator GetStandardInformationQueryResult(GetStandardForLarsCodeResponse source) =>
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
            Duration = source.Duration,
            DurationUnits = source.DurationUnits,
            IsRegulatedForProvider = source.IsRegulatedForProvider,
            CourseType = source.CourseType,
        };
}