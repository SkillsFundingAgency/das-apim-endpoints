using SFA.DAS.Apim.Shared.Common;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using DurationUnits = SFA.DAS.SharedOuterApi.Types.Constants.DurationUnits;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
public class GetCourseDetailsResponse
{
    public string StandardUId { get; set; }
    public string LarsCode { get; set; }
    public string IfateReferenceNumber { get; set; }
    public int Level { get; set; }
    public string Title { get; set; }
    public string ApprovalBody { get; set; }
    public bool IsRegulatedForProvider { get; set; }
    public int Duration { get; set; }
    public DurationUnits DurationUnits { get; set; }
    public string Route { get; set; }
    public LearningType ApprenticeshipType { get; set; }
    public CourseType CourseType { get; set; }
    public bool IsActiveAvailable { get; set; }
}