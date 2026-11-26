using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class GetAllShortCourseLearningsRequest(string ukprn, int academicYear, int page, int? pageSize) : IGetApiRequest
{
    public string GetUrl => $"/{ukprn}/academicyears/{academicYear}/shortcourses?page={page}&pageSize={pageSize}";
}
