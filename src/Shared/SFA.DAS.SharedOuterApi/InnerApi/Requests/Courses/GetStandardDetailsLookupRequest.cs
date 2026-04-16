using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetStandardDetailsLookupRequest(string id) : IGetApiRequest
{
    public string Id { get; } = id;
    public string GetUrl => $"api/courses/lookup/{Id}";
}