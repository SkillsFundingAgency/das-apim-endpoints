using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi;

public record GetCourseRequest(string StandardUid) : IGetApiRequest
{
    public string GetUrl => $"api/courses/standards/{StandardUid}";
}

public class GetCourseResponse
{
    public string StandardUId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new ();
    public List<string> Skills { get; set; } = new ();
    public List<KsbResponse> Ksbs { get; set; } = new ();
}

public class KsbResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}