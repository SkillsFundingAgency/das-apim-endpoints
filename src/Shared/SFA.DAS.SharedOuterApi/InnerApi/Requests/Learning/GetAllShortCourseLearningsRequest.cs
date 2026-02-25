using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

public class GetAllShortCourseLearningsRequest(string ukprn, int academicYear, int page, int? pageSize) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/academicyears/{academicYear}/shortcourses?page={page}&pageSize={pageSize}";
}