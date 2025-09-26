namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class ProviderCourseModel
{
    public string CourseName { get; set; }
    public int Level { get; set; }
    public int LarsCode { get; set; }
    public string IfateReferenceNumber { get; set; }

    public static implicit operator ProviderCourseModel(ProviderCourseResponse source)
    {
        return new ProviderCourseModel
        {
            CourseName = source.CourseName,
            Level = source.Level,
            LarsCode = source.LarsCode,
            IfateReferenceNumber = source.IfateReferenceNumber
        };
    }
}
