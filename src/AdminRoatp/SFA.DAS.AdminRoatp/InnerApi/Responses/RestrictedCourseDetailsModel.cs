using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.Constants;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Responses;

public class RestrictedCourseDetailsModel
{
    public string LarsCode { get; set; } = string.Empty;
    public string IfateReferenceNumber { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public LearningType LearningType { get; set; }
    public CourseType CourseType { get; set; }
    public bool IsActiveAvailable { get; set; }
    public DateTime? DateLastStarts { get; set; }
    public bool IsCourseRestricted { get; set; }
    public List<ProviderModel> Providers { get; set; } = [];
}
