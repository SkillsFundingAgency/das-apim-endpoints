using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetCourseCodesByUkprnRequest(long ukprn) : IGetApiRequest
{
    public long Ukprn { get; } = ukprn;
    public string GetUrl => $"providers/{Ukprn}/learners/coursecodes";
}