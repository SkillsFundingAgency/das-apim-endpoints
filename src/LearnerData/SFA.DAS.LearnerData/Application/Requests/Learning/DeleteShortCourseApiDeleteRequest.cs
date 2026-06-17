using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Application.Requests.Learning;

public class DeleteShortCourseApiDeleteRequest : IDeleteApiRequest
{
    public DeleteShortCourseApiDeleteRequest(long ukprn, Guid learnerKey)
    {
        Ukprn = ukprn;
        LearnerKey = learnerKey;
    }

    public long Ukprn { get; }
    public Guid LearnerKey { get; }
    public string DeleteUrl => $"{Ukprn}/shortCourses/{LearnerKey}";
}
