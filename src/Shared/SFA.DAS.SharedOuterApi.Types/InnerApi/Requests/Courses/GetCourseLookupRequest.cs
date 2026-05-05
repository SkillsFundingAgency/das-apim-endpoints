using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetCourseLookupRequest(string id) : IGetApiRequest
{
    public string Id { get; } = id;
    public string GetUrl => $"api/courses/lookup/{Id}";
}