using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetFrameworkRequest(string code) : IGetApiRequest
{
    public string FrameworkCode { get; } = code;
    public string GetUrl => $"api/courses/frameworks/{FrameworkCode}";
}