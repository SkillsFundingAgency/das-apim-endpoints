using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetKsbsForCourseOptionRequest(string larsCode) : IGetApiRequest
{
    public string LarsCode { get; } = larsCode;
    public string GetUrl => $"api/courses/lookup/{LarsCode}/options/all/ksbs";
}