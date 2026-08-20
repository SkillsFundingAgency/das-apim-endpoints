using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class RemoveLearnerApiDeleteRequest : IDeleteApiRequest
{
    public RemoveLearnerApiDeleteRequest(Guid learningKey, long ukprn, int academicYear)
    {
        LearningKey = learningKey;
        Ukprn = ukprn;
        AcademicYear = academicYear;
    }

    public Guid LearningKey { get; set; }
    public long Ukprn { get; set; }
    public int AcademicYear { get; set; }
    public string DeleteUrl => $"{Ukprn}/{LearningKey}?academicYear={AcademicYear}";
}