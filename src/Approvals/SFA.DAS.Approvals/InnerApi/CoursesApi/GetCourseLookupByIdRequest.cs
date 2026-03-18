using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CoursesApi;

public class GetCourseLookupByIdRequest(string id) : IGetApiRequest
{
    public string Id { get; } = id;
    public string GetUrl => $"api/courses/lookup/{Id}";
}