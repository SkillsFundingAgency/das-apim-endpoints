using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetStandardDetailsByIdRequest(string id) : IGetApiRequest
{
    public string Id { get; } = id;
    public string GetUrl => $"api/courses/standards/{Id}";
}