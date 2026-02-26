using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class ProviderCourseModel
{
    public string CourseName { get; set; }
    public CourseType CourseType { get; set; }
    public ApprenticeshipType ApprenticeshipType { get; set; }
    public int Level { get; set; }
    public string LarsCode { get; set; }
    public string IfateReferenceNumber { get; set; }

    public static implicit operator ProviderCourseModel(ProviderCourseResponse source)
    {
        return new ProviderCourseModel
        {
            CourseName = source.CourseName,
            CourseType = source.CourseType,
            ApprenticeshipType = source.ApprenticeshipType,
            Level = source.Level,
            LarsCode = source.LarsCode,
            IfateReferenceNumber = source.IfateReferenceNumber
        };
    }
}
