using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetKsbsForCourseOptionRequest(string larsCode) : IGetApiRequest
{
    public string LarsCode { get; } = larsCode;
    public string GetUrl => $"api/courses/standards/{LarsCode}/options/core/ksbs";
}