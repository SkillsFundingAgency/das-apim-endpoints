using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CoursesApi;

public class GetCourseVersionsRequest(string referenceNumber) : IGetApiRequest
{
    public string GetUrl => $"api/courses/lookup/versions/{referenceNumber}";
}
