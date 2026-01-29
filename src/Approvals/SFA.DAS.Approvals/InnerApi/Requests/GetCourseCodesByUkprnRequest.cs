using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class GetCourseCodesByUkprnRequest(long ukprn) : IGetApiRequest
{
    public string GetUrl => $"providers/{ukprn}/learners/coursecodes";
}