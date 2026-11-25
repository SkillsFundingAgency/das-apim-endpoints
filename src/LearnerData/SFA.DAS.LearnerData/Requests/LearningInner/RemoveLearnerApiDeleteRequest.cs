using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class RemoveLearnerApiDeleteRequest : IDeleteApiRequest
{
    public RemoveLearnerApiDeleteRequest(Guid learnerKey, long ukprn, int academicYear)
    {
        LearnerKey = learnerKey;
        Ukprn = ukprn;
        AcademicYear = academicYear;
    }

    public Guid LearnerKey { get; set; }
    public long Ukprn { get; set; }
    public int AcademicYear { get; set; }
    public string DeleteUrl => $"{Ukprn}/{LearnerKey}?academicYear={AcademicYear}";
}