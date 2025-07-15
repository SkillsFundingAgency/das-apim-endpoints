using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class GetAllLearningsRequest(string ukprn, int academicYear, int page, int? pageSize) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/academicyears/{academicYear}/learnings?page={page}&pageSize={pageSize}";
}