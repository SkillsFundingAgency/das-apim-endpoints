using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetKsbsForCourseOptionRequest(string larsCode) : IGetApiRequest
{
    public string LarsCode { get; } = larsCode;
    public string GetUrl => $"api/courses/standards/{LarsCode}/options/core/ksbs";
}