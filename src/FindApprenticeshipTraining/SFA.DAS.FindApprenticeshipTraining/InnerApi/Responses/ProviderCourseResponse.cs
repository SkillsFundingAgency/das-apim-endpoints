using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class ProviderCourseResponse
{
    public string CourseName { get; set; }
    public CourseType CourseType { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public int Level { get; set; }
    public string LarsCode { get; set; }
    public string IfateReferenceNumber { get; set; }
}