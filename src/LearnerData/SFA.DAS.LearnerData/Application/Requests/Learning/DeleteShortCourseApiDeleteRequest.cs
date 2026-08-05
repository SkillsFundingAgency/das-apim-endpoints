using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Learning;

public class DeleteShortCourseApiDeleteRequest : IDeleteApiRequest
{
    public DeleteShortCourseApiDeleteRequest(long ukprn, Guid learnerKey, int academicYear)
    {
        Ukprn = ukprn;
        LearnerKey = learnerKey;
        AcademicYear = academicYear;
    }

    public long Ukprn { get; }
    public Guid LearnerKey { get; }
    public int AcademicYear { get; }
    public string DeleteUrl => $"{Ukprn}/shortCourses/{LearnerKey}?academicYear={AcademicYear}";
}
