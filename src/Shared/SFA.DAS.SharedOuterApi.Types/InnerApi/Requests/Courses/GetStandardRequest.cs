using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

public class GetStandardRequest(int id) : IGetApiRequest
{
    public int StandardId { get; } = id;
    public string GetUrl => $"api/courses/standards/{StandardId}";
}