using SFA.DAS.Common.Domain.Types;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.InnerApi.Models;

public class StandardModel
{
    public string StandardUId { get; set; } = string.Empty;
    public string LarsCode { get; set; } = string.Empty;
    public string IfateReferenceNumber { get; set; } = string.Empty;
    public int Level { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ApprovalBody { get; set; } = string.Empty;
    public bool IsRegulatedForProvider { get; set; }
    public string Route { get; set; } = string.Empty;
    public LearningType LearningType { get; set; }
    public CourseType CourseType { get; set; }
    public bool IsActiveAvailable { get; set; }
}
